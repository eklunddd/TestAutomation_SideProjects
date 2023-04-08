using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RegresTesting_Sandbox
{
    class Utility
    {
        private static string _regresApi = "https://reqres.in";

        public string CreateRandomName()
        {
            string[] firstNames = { "Anton", "Janne", "Bob", "Alice", "Kalle", "Emelie", "Sanna", "Moa", "Sandra" };
            string[] lastNames = { "Eklund", "Johansson", "Falk", "Karlsson", "Jonsson", "Björk" };

            var random = new Random();
            var firstName = firstNames[random.Next(0, firstNames.Length)];
            var lastName = lastNames[random.Next(0, lastNames.Length)];

            return $"{firstName} {lastName}";
        }

        public async Task<CreateUserRequestDto> CreateUserDtoObject()
        {
            string name = CreateRandomName();
            return new CreateUserRequestDto
            {
                Name = name,
                Email = name + "@mailExample.com"
            };
        }



        public async Task<CreateUserResponseDto> CreateNewUserReturnResponseDto(CreateUserRequestDto userDto)
        {
            using var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(userDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{_regresApi}/api/users", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var returnedDto = JsonConvert.DeserializeObject<CreateUserResponseDto>(responseContent);
            return returnedDto;
        }
        public async Task<ExistingUserDto> GetUserById(int userId)
        {
            int maxRetries = 10;
            int retryIntervalMs = 1000;
            int timeoutMs = 10000;
            using var httpClient = new HttpClient();
            var startTime = DateTime.UtcNow;
            while (DateTime.UtcNow - startTime < TimeSpan.FromMilliseconds(timeoutMs))
            {
                var response = await httpClient.GetAsync($"{_regresApi}/users/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var userDto = JsonConvert.DeserializeObject<ExistingUserDto>(responseContent);
                    return userDto;
                }
                else
                {
                    maxRetries--;
                    if (maxRetries <= 0)
                    {
                        break;
                    }
                    await Task.Delay(retryIntervalMs);
                }
            }
            return null;
        }
    }
}
