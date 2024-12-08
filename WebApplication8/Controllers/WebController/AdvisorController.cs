using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WebApplication8.DTO;
using WebApplication8.Models;

namespace WebApplication8.Controllers.WebController
{
    public class AdvisorController : Controller
    {
        private readonly HttpClient _httpClient;

        // IHttpClientFactory DI ile inject ediliyor
        public AdvisorController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
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
            var apiUrl = $"https://localhost:7276/api/advisors/{id}";

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
                var student = JsonConvert.DeserializeObject<Advisors>(jsonResponse);

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
        public async Task<IActionResult> AllLessons()
        {
            try
            {
                // API'ye istek gönderiyoruz
                var response = await _httpClient.GetAsync("https://localhost:7276/api/courses");

                // Eğer başarılı bir yanıt alırsak
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    // API'den gelen JSON verisini Lesson modeline deserialize ediyoruz
                    var lessons = JsonConvert.DeserializeObject<List<Course>>(data);

                    // Veriyi View'a gönderiyoruz
                    return View(lessons);
                }

                // API'den veri çekilemezse hata sayfasına yönlendirebiliriz
                return View("Error");
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching lessons: " + ex.Message;
                return View("Error");
            }
        }
        public async Task<IActionResult> AllUsers()
        {
            try
            {
                // API'ye istek gönderiyoruz
                var response = await _httpClient.GetAsync("https://localhost:7276/api/users");

                // Yanıtın başarılı olup olmadığını kontrol et
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching data. Status Code: " + response.StatusCode;
                    return View("Error");
                }

                // API'den gelen veriyi string olarak al
                var data = await response.Content.ReadAsStringAsync();

                // Gelen veriyi loglamak, hata durumunu tespit etmek için
                Console.WriteLine("API Response Data: " + data);

                // Eğer veri boşsa, hata mesajı göster
                if (string.IsNullOrEmpty(data))
                {
                    ViewBag.ErrorMessage = "API returned empty data.";
                    return View("Error");
                }

                // JSON verisini Student modeline deserialize ediyoruz
                var students = JsonConvert.DeserializeObject<List<Users>>(data);

                // Eğer deserialize başarılı olduysa veriyi View'a gönder
                if (students != null && students.Any())
                {
                    return View(students);
                }
                else
                {
                    ViewBag.ErrorMessage = "No students found in the data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }
        public async Task<IActionResult> SettingsLesson()
        {
            try
            {
                // API'ye istek gönderiyoruz
                var response = await _httpClient.GetAsync("https://localhost:7276/api/courses");

                // Yanıtın başarılı olup olmadığını kontrol et
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching data. Status Code: " + response.StatusCode;
                    return View("Error");
                }

                // API'den gelen veriyi string olarak al
                var data = await response.Content.ReadAsStringAsync();

                // Gelen veriyi loglamak, hata durumunu tespit etmek için
                Console.WriteLine("API Response Data: " + data);

                // Eğer veri boşsa, hata mesajı göster
                if (string.IsNullOrEmpty(data))
                {
                    ViewBag.ErrorMessage = "API returned empty data.";
                    return View("Error");
                }

                // JSON verisini Student modeline deserialize ediyoruz
                var students = JsonConvert.DeserializeObject<List<Course>>(data);

                // Eğer deserialize başarılı olduysa veriyi View'a gönder
                if (students != null && students.Any())
                {
                    return View(students);
                }
                else
                {
                    ViewBag.ErrorMessage = "No students found in the data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var url = $"https://localhost:7276/api/courses/{courseId}";

            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Başarılı silme işlemi sonrası mesaj
                ViewBag.Message = "Ders başarıyla silindi.";
                ViewBag.MessageType = "success";
            }
            else
            {
                // Hatalı silme işlemi sonrası mesaj
                ViewBag.Message = "Ders silinirken bir hata oluştu.";
                ViewBag.MessageType = "error";
            }

            return RedirectToAction("SettingsLesson", "Advisor");
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                // API'ye ders ekleme isteği gönder
                var url = "https://localhost:7276/api/courses"; // API endpointi
                var jsonContent = JsonConvert.SerializeObject(course);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarıyla eklenen ders mesajı
                    ViewBag.Message = "Ders başarıyla eklendi.";
                    ViewBag.MessageType = "success";
                }
                else
                {
                    // Ders eklerken hata mesajı
                    ViewBag.Message = "Ders eklenirken bir hata oluştu.";
                    ViewBag.MessageType = "error";
                }
            }
            else
            {
                // Model geçerli değilse hata mesajı
                ViewBag.Message = "Ders bilgileri geçerli değil.";
                ViewBag.MessageType = "error";
            }

            // Ders yönetimi sayfasına yönlendir
            return RedirectToAction("SettingsLesson", "Advisor");
        }
        public async Task<IActionResult> AllStudents()
        {
            try
            {
                // API'ye istek gönderiyoruz
                var response = await _httpClient.GetAsync("https://localhost:7276/api/students");

                // Yanıtın başarılı olup olmadığını kontrol et
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching data. Status Code: " + response.StatusCode;
                    return View("Error");
                }

                // API'den gelen veriyi string olarak al
                var data = await response.Content.ReadAsStringAsync();

                // Gelen veriyi loglamak, hata durumunu tespit etmek için
                Console.WriteLine("API Response Data: " + data);

                // Eğer veri boşsa, hata mesajı göster
                if (string.IsNullOrEmpty(data))
                {
                    ViewBag.ErrorMessage = "API returned empty data.";
                    return View("Error");
                }

                // JSON verisini Student modeline deserialize ediyoruz
                var students = JsonConvert.DeserializeObject<List<Student>>(data);

                // Eğer deserialize başarılı olduysa veriyi View'a gönder
                if (students != null && students.Any())
                {
                    return View(students);
                }
                else
                {
                    ViewBag.ErrorMessage = "No students found in the data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromForm] Student course)
        {
            if (ModelState.IsValid)
            {
                // API'ye ders ekleme isteği gönder
                var url = "https://localhost:7276/api/Students"; // API endpointi
                var jsonContent = JsonConvert.SerializeObject(course);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarıyla eklenen ders mesajı
                    ViewBag.Message = "Ders başarıyla eklendi.";
                    ViewBag.MessageType = "success";
                }
                else
                {
                    // Ders eklerken hata mesajı
                    ViewBag.Message = "Ders eklenirken bir hata oluştu.";
                    ViewBag.MessageType = "error";
                }
            }
            else
            {
                // Model geçerli değilse hata mesajı
                ViewBag.Message = "Ders bilgileri geçerli değil.";
                ViewBag.MessageType = "error";
            }

            // Ders yönetimi sayfasına yönlendir
            return RedirectToAction("AllStudents", "Advisor");
        }



        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var url = $"https://localhost:7276/api/students/{studentId}"; // Doğru endpoint

            var response = await _httpClient.DeleteAsync(url);
            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                // Başarılı silme işlemi sonrası mesaj
                ViewBag.Message = "Öğrenci başarıyla silindi.";
                ViewBag.MessageType = "success";
            }
            else
            {
                // Hatalı silme işlemi sonrası mesaj
                ViewBag.Message = "Öğrenci silinirken bir hata oluştu.";
                ViewBag.MessageType = "error";
            }

            return RedirectToAction("AllStudents", "Advisor");
        }
        public async Task<IActionResult> SetAdvisor()
        {
            try
            {
                // API'ye istek gönderiyoruz
                var response = await _httpClient.GetAsync("https://localhost:7276/api/advisors");

                // Yanıtın başarılı olup olmadığını kontrol et
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching data. Status Code: " + response.StatusCode;
                    return View("Error");
                }

                // API'den gelen veriyi string olarak al
                var data = await response.Content.ReadAsStringAsync();

                // Gelen veriyi loglamak, hata durumunu tespit etmek için
                Console.WriteLine("API Response Data: " + data);

                // Eğer veri boşsa, hata mesajı göster
                if (string.IsNullOrEmpty(data))
                {
                    ViewBag.ErrorMessage = "API returned empty data.";
                    return View("Error");
                }

                // JSON verisini Student modeline deserialize ediyoruz
                var students = JsonConvert.DeserializeObject<List<Advisors>>(data);

                // Eğer deserialize başarılı olduysa veriyi View'a gönder
                if (students != null && students.Any())
                {
                    return View(students);
                }
                else
                {
                    ViewBag.ErrorMessage = "No students found in the data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteInstructor(int instructorId)
        {
            var url = $"https://localhost:7276/api/Advisors/{instructorId}"; // Doğru endpoint

            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Başarılı silme işlemi sonrası mesaj
                ViewBag.Message = "Öğrenci başarıyla silindi.";
                ViewBag.MessageType = "success";
            }
            else
            {
                // Hatalı silme işlemi sonrası mesaj
                ViewBag.Message = "Öğrenci silinirken bir hata oluştu.";
                ViewBag.MessageType = "error";
            }

            return RedirectToAction("SetAdvisor", "Advisor");
        }
        [HttpPost]
        public async Task<IActionResult> AddInstructor([FromForm] Advisors course)
        {
            if (ModelState.IsValid)
            {
                // API'ye ders ekleme isteği gönder
                var url = "https://localhost:7276/api/Advisors"; // API endpointi
                var jsonContent = JsonConvert.SerializeObject(course);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarıyla eklenen ders mesajı
                    ViewBag.Message = "Ders başarıyla eklendi.";
                    ViewBag.MessageType = "success";
                }
                else
                {
                    // Ders eklerken hata mesajı
                    ViewBag.Message = "Ders eklenirken bir hata oluştu.";
                    ViewBag.MessageType = "error";
                }
            }
            else
            {
                // Model geçerli değilse hata mesajı
                ViewBag.Message = "Ders bilgileri geçerli değil.";
                ViewBag.MessageType = "error";
            }

            // Ders yönetimi sayfasına yönlendir
            return RedirectToAction("SetAdvisor", "Advisor");
        }
        public async Task<IActionResult> AddUser()
        {
            try
            {
                // API'ye istek gönderiyoruz
                var response = await _httpClient.GetAsync("https://localhost:7276/api/users");

                // Yanıtın başarılı olup olmadığını kontrol et
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Error fetching data. Status Code: " + response.StatusCode;
                    return View("Error");
                }

                // API'den gelen veriyi string olarak al
                var data = await response.Content.ReadAsStringAsync();

                // Gelen veriyi loglamak, hata durumunu tespit etmek için
                Console.WriteLine("API Response Data: " + data);

                // Eğer veri boşsa, hata mesajı göster
                if (string.IsNullOrEmpty(data))
                {
                    ViewBag.ErrorMessage = "API returned empty data.";
                    return View("Error");
                }

                // JSON verisini Student modeline deserialize ediyoruz
                var students = JsonConvert.DeserializeObject<List<Users>>(data);

                // Eğer deserialize başarılı olduysa veriyi View'a gönder
                if (students != null && students.Any())
                {
                    return View(students);
                }
                else
                {
                    ViewBag.ErrorMessage = "No students found in the data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını view'a gönder
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var url = $"https://localhost:7276/api/Users/{userId}"; // Doğru endpoint

            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Başarılı silme işlemi sonrası mesaj
                ViewBag.Message = "Öğrenci başarıyla silindi.";
                ViewBag.MessageType = "success";
            }
            else
            {
                // Hatalı silme işlemi sonrası mesaj
                ViewBag.Message = "Öğrenci silinirken bir hata oluştu.";
                ViewBag.MessageType = "error";
            }

            return RedirectToAction("AddUser", "Advisor");
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromForm] Users course)
        {
            if (ModelState.IsValid)
            {
                // API'ye ders ekleme isteği gönder
                var url = "https://localhost:7276/api/Users"; // API endpointi
                var jsonContent = JsonConvert.SerializeObject(course);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarıyla eklenen ders mesajı
                    ViewBag.Message = "Ders başarıyla eklendi.";
                    ViewBag.MessageType = "success";
                }
                else
                {
                    // Ders eklerken hata mesajı
                    ViewBag.Message = "Ders eklenirken bir hata oluştu.";
                    ViewBag.MessageType = "error";
                }
            }
            else
            {
                // Model geçerli değilse hata mesajı
                ViewBag.Message = "Ders bilgileri geçerli değil.";
                ViewBag.MessageType = "error";
            }

            // Ders yönetimi sayfasına yönlendir
            return RedirectToAction("AddUser", "Advisor");
        }
        public async Task<IActionResult> CoursesConfirm()
        {
            var apiUrl = "https://localhost:7276/api/nonconfirmedselections"; // API adresinizi buraya yazın

            List<NonConfirmedSelectionDto> nonConfirmedSelections = new List<NonConfirmedSelectionDto>();

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    nonConfirmedSelections = JsonConvert.DeserializeObject<List<NonConfirmedSelectionDto>>(jsonResponse);
                }
                else
                {
                    ViewBag.ErrorMessage = "API'den veri alınamadı.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"API bağlantısında bir hata oluştu: {ex.Message}";
            }

            return View(nonConfirmedSelections);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveSelection(int id, int studentId, int courseId)
        {
            var selection = new StudentCourseSelections
            {
                StudentID = studentId,
                CourseID = courseId,
                SelectionDate = DateTime.Now,
                IsApproved = true
            };

            // API URL'si
            var apiUrl = "https://localhost:7276/api/StudentCourseSelections";

            // API'ye POST isteği yapma
            var jsonData = JsonConvert.SerializeObject(selection);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var deleteUrl = $"https://localhost:7276/api/NonConfirmedSelections/{id}";
                var deleteResponse = await _httpClient.DeleteAsync(deleteUrl);

                if (deleteResponse.IsSuccessStatusCode)
                {
                    // Başarıyla silindiğinde Kurs Onaylama Sayfasına Yönlendir
                    return RedirectToAction("CoursesConfirm");
                }
                else
                {
                    // Silme işlemi başarısız olduğunda
                    ViewBag.ErrorMessage = "Ders seçimi onaylandı ancak silinemedi.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Ders seçimi onaylanamadı.";
                return View();
            }
        }




    }



}

