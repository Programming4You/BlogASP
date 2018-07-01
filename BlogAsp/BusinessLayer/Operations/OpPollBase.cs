using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.DataLayer;

namespace BlogAsp.BusinessLayer.Operations
{

    public class OpPollBase : Operation
    {
   
        public class PollCriteria : SelectCriteria
        {
            public int? IdPollQuestion { get; set; }
        }


        public PollCriteria Criteria { get; set; }

        public override OperationResult Execute(BlogEntities entiteti)
        {
            IEnumerable<PollAnswersDto> iePollAnswers;
            PollDto[] iePollQuestions;
            if (Criteria != null)
            {
                iePollQuestions = (from pollq in entiteti.PollQuestions
                                   where (Criteria.IdPollQuestion != null ? true : Criteria.IdPollQuestion == pollq.idPollQuestion)
                                   select new PollDto()
                                   {
                                       IdPollQuestion = pollq.idPollQuestion,
                                       PollQuestion = pollq.question
                                   }).ToArray();
                iePollAnswers = (from polla in entiteti.PollAnswers
                                 where (polla.idPollQuestion == iePollQuestions[0].IdPollQuestion)
                                 select new PollAnswersDto()
                                 {
                                     IdPollAnswer = polla.idPollAnswer,
                                     PollAnswer = polla.answer
                                 });
                iePollQuestions[0].answers = iePollAnswers.ToArray();
            }
            else
            {

                iePollQuestions = (from pollq in entiteti.PollQuestions
                                   select new PollDto()
                                   {
                                       IdPollQuestion = pollq.idPollQuestion,
                                       PollQuestion = pollq.question
                                   }).OrderBy(x => Guid.NewGuid()).Take(1).ToArray();

                int id = iePollQuestions[0].IdPollQuestion;
                iePollAnswers = (from polla in entiteti.PollAnswers
                                 where (polla.idPollQuestion == id)
                                 select new PollAnswersDto()
                                 {
                                     IdPollAnswer = polla.idPollAnswer,
                                     PollAnswer = polla.answer
                                 });
                iePollQuestions[0].answers = iePollAnswers.ToArray();
            }

            OperationResult result = new OperationResult() { Status = true, Message = "Poll select" };


            result.Items = iePollQuestions.ToArray();

            return result;
        }
    }

    public class OpPollSelect : OpPollBase
    {

    }



    public class OpPollVoteInsert : OpPollBase
    {
        public PollDto Vote { get; set; }

        public override OperationResult Execute(BlogEntities entiteti)
        {
            int num = (from votes in entiteti.PollVotes
                join answer in entiteti.PollAnswers on votes.idAnswer equals answer.idPollAnswer
                where Vote.IpAddress == votes.ipAddress && Vote.IdQuestion == answer.idPollQuestion
                select votes).Count();
            OperationResult result = new OperationResult() { Message = "Inserting poll vote" };
            if (num == 0)
            {
                entiteti.spInsertPollVote(Vote.IpAddress, Vote.IdAnswer);
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }

            return result;


        }

    }



    //Admin

    public class OpPollQuestionSelect : OpPollBase
    {
        public PollCriteria Criteria { get; set; }
        public override OperationResult Execute(BlogEntities entities)
        {
            IEnumerable<PollDto> iePollQuestions;

            if (Criteria != null)
            {
                iePollQuestions = from pq in entities.PollQuestions
                                  where (Criteria.IdPollQuestion == null ? true : Criteria.IdPollQuestion == pq.idPollQuestion)
                                  select new PollDto()
                                  {
                                      IdPollQuestion = pq.idPollQuestion,
                                      PollQuestion = pq.question
                                  };
            }
            else
            {
                iePollQuestions = from pq in entities.PollQuestions
                                  select new PollDto()
                                  {
                                      IdPollQuestion = pq.idPollQuestion,
                                      PollQuestion = pq.question
                                  };
            }

            OperationResult result = new OperationResult() { Status = true, Message = "Poll select" };

            result.Items = iePollQuestions.ToArray();


            return result;
        }
    }





    public class OpPollDeleteQuestion : OpPollBase
    {
        public PollDto PollQuestion { get; set; }
        public override OperationResult Execute(BlogEntities entities)
        {
            if (PollQuestion != null)
                entities.spDeletePollQuestion(PollQuestion.IdPollQuestion);
            return base.Execute(entities);
        }
    }








}