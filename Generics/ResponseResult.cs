namespace MyRestAPI.Generics
{
    //STEP 9: HERE WE CREATE GENERICS FOLDER, TO DEFINE GENERICS. WITH THE HELP OF GENERICS, WE GET A JSON FORMAT DETAILED INFORMATION , WHEN WE TESTING THE API ON SWAGGER OR POSTMAN, INSTEAD OF GETTING ONLY SINGLE INCOMPLETE INFORMATION, WE CAN GET THE INFORMATION USING JSON FORMAT API: 
    public class ResponseResult<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public Boolean Status { get; set; } = false; //MAKE DEFAULT AS PASS, SO THAT WHEN WE GET RESPONSE, IT SHOWS TRUE, FOR BETTER UNDERSTAING. 



        //NOW DEFINING THE METHODS WHEN TO SHOW FAILURE AND WHEN TO SUCCESS: 
        public static ResponseResult<T> Success(T? data, string Message)
        {
            return new ResponseResult<T>
            {
                Data = data,
                Message = Message,
                Status = true
            };
        }

        public static ResponseResult<T> Failure(T? data, string Message)
        {
            return new ResponseResult<T>
            {
                Data = data,
                Message = Message,
                Status = false
            };
        }

    //NOW ONCE DEFINED THE GENERICS, NOW GO TO AUTHCONTROLLER, AND REPALCE THE CONDITIONS WITH GENERICS, USING THIS CLASS METHODS... 
    }
}
