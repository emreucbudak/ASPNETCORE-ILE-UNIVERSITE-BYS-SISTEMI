using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication8.Data;
using WebApplication8.Models;

using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebApplication8.Controllers.WebController
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly RepositoryDbContext _repositoryDbContext;

        // IHttpClientFactory DI ile inject ediliyor
        public StudentController(IHttpClientFactory httpClientFactory, RepositoryDbContext repositoryDbContext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _repositoryDbContext = repositoryDbContext;
        }

        // ID'ye göre öğrenci verilerini API'den alıyoruz
        public async Task<IActionResult> Index(int id)
        {
            // API endpoint URL'si
            var userId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LoginUser", "Account");
            }
            var apiUrl = $"https://localhost:7276/api/students/{id}";

            try
            {
                // API'den öğrenci verilerini al
                var response = await _httpClient.GetAsync(apiUrl);

                // Eğer HTTP yanıtı başarılı değilse, hata mesajı döndür
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching student data. Status Code: " + response.StatusCode;
                    return View("Error"); // Hata sayfasına yönlendirme
                }

                // Başarılı yanıt, JSON verisini al
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // JSON verisini Student modeline deserialize et
                var student = JsonConvert.DeserializeObject<Student>(jsonResponse);

                // Öğrenci verisini view'a gönder
                return View(student);
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }
        public async Task<IActionResult> PersonalLessons()
        {
            try
            {
                var x = int.Parse(HttpContext.Session.GetString("StudentID"));

                // API'ye istek gönderiyoruz
                var response = await _httpClient.GetAsync($"https://localhost:7276/api/studentcourseselections/{x}");


                // Eğer başarılı bir yanıt alırsak
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    // API'den gelen JSON verisini Lesson modeline deserialize ediyoruz
                    var lessons = JsonConvert.DeserializeObject<List<StudentCourseSelections>>(data);

                    // Veriyi View'a gönderiyoruz
                    return View(lessons);
                }

                // API'den veri çekilemezse hata sayfasına yönlendirebiliriz
                return View("NoCoursesFound");
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching lessons: " + ex.Message;
                return View("Error");
            }
        }
        public async Task<IActionResult> PickLessons()
        {
            var userId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userId))
            {
                // Eğer kullanıcı ID'si oturumda bulunamazsa, login sayfasına yönlendirebilirsiniz.
                return RedirectToAction("Login");
            }

            int studentID;
            if (!int.TryParse(userId, out studentID))
            {
                // Eğer öğrenci ID'si geçersizse, hata mesajı verebilirsiniz.
                return BadRequest("Geçersiz kullanıcı ID'si.");
            }

            // API'ye öğrenci ID'sini gönder
            var selectionHistory = await CheckCourseSelectionHistoryApi(studentID);

            if (selectionHistory != null)
            {
                // Eğer seçim geçmişi varsa, tekrar seçim yapmasına izin vermeyin
                return RedirectToAction("ConfirmedSelected", "Student");
            }

            // Seçim geçmişi yoksa, kursları almak için API'ye istekte bulunun
            var courses = await GetCoursesFromApi();

            return View(courses);
        }
        private async Task<CourseSelectionHistory> CheckCourseSelectionHistoryApi(int studentID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7276/api/CourseSelectionHistories/{studentID}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var selectionHistory = JsonConvert.DeserializeObject<CourseSelectionHistory>(content);

                return selectionHistory;
            }

            // Eğer API'den veri gelmezse veya hata alırsak, null döndürebiliriz.
            return null;
        }
        private async Task<List<Course>> GetCoursesFromApi()
        {
            var response = await _httpClient.GetAsync("https://localhost:7276/api/courses");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(jsonString);
                return courses;
            }

            return new List<Course>(); // Eğer API başarısız olursa boş bir liste döndür
        }
        public IActionResult AlreadySelect()
        {
            return View();
        }
[HttpPost]
public async Task<IActionResult> SubmitCourseSelections(List<int> selectedCourseIds)
{
    int a;
    string courseQuotaApiUrl = "https://localhost:7276/api/coursequotas/";

    // Kontenjanları kontrol et
    foreach (var courseId in selectedCourseIds)
    {
        // Kontenjan API'sine istek gönderiyoruz
        var response = await _httpClient.GetAsync($"{courseQuotaApiUrl}{courseId}");

        if (!response.IsSuccessStatusCode)
        {
            // Eğer kontenjan API'si başarısız ise, yönlendiriyoruz
            TempData["ErrorMessage"] = "Kontenjan dolmuş, lütfen başka bir ders seçin.";
            return RedirectToAction("PickLessons", "Student");
        }

        // CourseQuotaResponse modelini kullanarak gelen veriyi deserialize ediyoruz
        var quotaResponse = await response.Content.ReadFromJsonAsync<CourseQuotaResponse>();

        if (quotaResponse != null)
        {
            int quota = quotaResponse.Quota; // toplam kontenjan
            int remainingQuota = quotaResponse.RemainingQuota;
                    a = quotaResponse.RemainingQuota;// kalan kontenjan

            // Eğer kontenjan sıfır ise, kullanıcıyı bilgilendir
            if (remainingQuota <= 0)
            {
                TempData["ErrorMessage"] = "Kontenjan dolmuş, lütfen başka bir ders seçin.";
                return RedirectToAction("PickLessons", "Student");
            }
        }
    }

    // Kontenjanlar uygun, NonConfirmedSelections API'sine veri gönder
    foreach (var courseId in selectedCourseIds)
    {
                var nonConfirmedSelection = new NonConfirmedSelections
                {
                    StudentId = int.Parse(HttpContext.Session.GetString("StudentID")),
                    CourseId = courseId,
                    SelectedAt = DateTime.Now
                };


                // NonConfirmedSelections API'sine veri gönderiyoruz
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7276/api/nonconfirmedselections", nonConfirmedSelection);

        if (!response.IsSuccessStatusCode)
        {
            // API'ye veri gönderme başarısızsa kullanıcıyı bilgilendir
            TempData["ErrorMessage"] = "Seçiminiz kaydedilemedi, tekrar deneyin.";
            return RedirectToAction("PickLessons", "Student");
        }
    }

            // Tüm seçimler başarılıysa, kontenjanı güncelle
            foreach (var courseId in selectedCourseIds)
            {
                // Kontenjan bilgisini almak için API'yi çağırıyoruz
                var response = await _httpClient.GetAsync($"{courseQuotaApiUrl}{courseId}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Kontenjan bilgisi alınamadı, tekrar deneyin.";
                    return RedirectToAction("PickLessons", "Student");
                }

                // Gelen yanıtı `CourseQuotaResponse` modeline deserialize ediyoruz
                var quotaResponse = await response.Content.ReadFromJsonAsync<CourseQuotaResponse>();

                if (quotaResponse != null)
                {
                    // Kontenjanı bir azaltıyoruz
                    var courseQuota = new
                    {
                        RemainingQuota = quotaResponse.RemainingQuota - 1  // Güncellenmiş kontenjan değeri
                    };

                    // JSON verisine dönüştürüyoruz
                    var content = new StringContent(
                        JsonConvert.SerializeObject(courseQuota),  // JSON'a dönüştürülmüş veri
                        Encoding.UTF8,
                        "application/json"
                    );

                    // PATCH isteğini gönderiyoruz
                    var updateQuotaResponse = await _httpClient.PatchAsync($"{courseQuotaApiUrl}coursequotas/{courseId}", content);

                    if (!updateQuotaResponse.IsSuccessStatusCode)
                    {
                        TempData["ErrorMessage"] = "Kontenjan güncellenemedi, tekrar deneyin.";
                        return RedirectToAction("PickLessons", "Student");
                    }
                }
            }
            foreach (var courseId in selectedCourseIds)
            {
                int studentId = int.Parse(HttpContext.Session.GetString("StudentID"));
                string courseSelectionHistoryApiUrl = $"https://localhost:7276/api/CourseSelectionHistories";
                // Öğrencinin ders seçim bilgisi
                var courseSelectionHistory = new CourseSelectionHistory
                {
                    StudentID = studentId,
                    SelectionDate = DateTime.Now
                };

                // API'ye veri gönderme
                var response = await _httpClient.PostAsJsonAsync(courseSelectionHistoryApiUrl, courseSelectionHistory);

                if (!response.IsSuccessStatusCode)
                {
                    // API'ye veri gönderme başarısızsa kullanıcıyı bilgilendir
                    TempData["ErrorMessage"] = "Seçiminiz kaydedilemedi, tekrar deneyin.";
                    return RedirectToAction("PickLessons", "Student");
                }
            }



            // Her şey başarılıysa, başarı mesajı ve yönlendirme
            TempData["SuccessMessage"] = "Ders seçiminiz başarıyla kaydedildi!";
            return RedirectToAction("ConfirmedSelected", "Student");
}
        public IActionResult ConfirmedSelected ()
        {
            return View();
        }







    }
}

