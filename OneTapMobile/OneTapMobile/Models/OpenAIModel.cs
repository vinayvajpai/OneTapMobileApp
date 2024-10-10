using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    //Response model
    public class Choice
    {
        public string text { get; set; }
        public int index { get; set; }
        public object logprobs { get; set; }
        public string finish_reason { get; set; }
    }

    public class OpenAIModel
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public List<Choice> choices { get; set; }
        public Usage usage { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    //Request model
    public class AIRequestModel
    {
        public string model { get; set; } = "text-davinci-003";
        public string prompt { get; set; }
        public double temperature { get; set; } = 0.5;
        public int max_tokens { get; set; } = 200;
    }
   
    //Error model
    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
        public object param { get; set; }
        public object code { get; set; }
    }
    public class RootErrorModel
    {
        public Error error { get; set; }
    }

    //store OpenAI data model

    public class GoAdResponse
    {
        public string headline_1 { get; set; }
        public string headline_2 { get; set; }
        public string headline_3 { get; set; }
        public string description_1 { get; set; }
        public string description_2 { get; set; }
    }
    public class FBAdResponse
    {
        public string headline { get; set; }
        public string primarytext { get; set; }
        //public string shortdesciption { get; set; }
    }
}
