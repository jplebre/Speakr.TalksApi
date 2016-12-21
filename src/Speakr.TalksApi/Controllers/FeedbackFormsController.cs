﻿using Microsoft.AspNetCore.Mvc;
using Speakr.TalksApi.DataAccess;
using Speakr.TalksApi.Models.FeedbackForm;
using System.Threading.Tasks;
using Speakr.TalksApi.Swagger;

namespace Speakr.TalksApi.Controllers
{
    [Route("talks")]
    public class FeedbackFormsController : Controller
    {
        private readonly IRepository _dbRepository;

        public FeedbackFormsController(IRepository repository)
        {
            _dbRepository = repository;
        }

        [HttpGet]
        [Produces(typeof(FeedbackForm))]
        [Route("{easyAccessKey}/FeedbackForm")]
        [SwaggerSummary("Get FeedbackForm by easy access key (string)")]
        [SwaggerNotes("Url: /talks/{easyAccessKey}/FeedbackForm")]
        public async Task<IActionResult> GetFeedbackFormForTalk(string easyAccessKey)
        {
            var talk = _dbRepository.GetTalkByEasyAccessKey(easyAccessKey);

            if (talk == null)
                return NotFound($"Could not find talk with easy access key: {easyAccessKey}");

            var questionnaire = _dbRepository.GetQuestionnaire(talk.Id);

            if(questionnaire == null)
                return NotFound($"Could not find feedback form with talk id key: {talk.Id}");

            var feedbackForm = new FeedbackForm();

            feedbackForm.TalkId = talk.Id;
            feedbackForm.TalkName = talk.Name;
            feedbackForm.SpeakerName = talk.SpeakerName;
            feedbackForm.EasyAccessKey = talk.EasyAccessKey;
            feedbackForm.Questionnaire = questionnaire;
            feedbackForm.Description = talk.Description;

            return Ok(feedbackForm);
        }

        // {easyAccessKey}/FeedbackForm should be to EDIT/UPDATE feedback forms
        // Add that controller action here
    }
}
