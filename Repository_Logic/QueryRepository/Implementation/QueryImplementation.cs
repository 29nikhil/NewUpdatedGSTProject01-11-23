﻿using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.Dto;
using Repository_Logic.QueryRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.QueryRepository.Implementation
{
    public class QueryImplementation : IQuery
    {
        private readonly Application_Db_Context _context;
        private UserManager<IdentityUser> _IdentityUserManager;
        public QueryImplementation(Application_Db_Context context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager)
        {
            _context = context;
            _IdentityUserManager = IdentityUserManager;
        }

        public async Task<List<Query_Dto>> GetAllQueries(string LoginSessionID)
        {
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "User");
            List<Query_Dto> queries = new List<Query_Dto>();
            if (isInRole)
            {
                queries = _context.queries.Where(p => p.UserID == LoginSessionID).OrderBy(p => p.AnsweredQuestionDate).Select(data => new Query_Dto
                {
                    Id = data.Id,
                    UserID = data.UserID,
                    UserName = _IdentityUserManager.Users.Where(x => x.Id == data.UserID).Select(x => x.UserName).FirstOrDefault(),
                    Question = data.Question,
                    Answer = data.Answer,
                    AnsweredBy = data.AnsweredBy == "Not Answered By CA" ? "Not Answered By CA" : _IdentityUserManager.Users.Where(x => x.Id == data.AnsweredBy).Select(x => x.UserName).FirstOrDefault(),
                    QuestionAskedDate = data.QuestionAskedDate,
                    QuestionDate = data.QuestionAskedDate.ToString("dd/MM/yyyy hh:mm tt"),
                    AnswerDate = data.AnsweredQuestionDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt"),
                    AnsweredQuestionDate = data.AnsweredQuestionDate
                }).ToList();
            }
            else
            {
                queries = _context.queries.OrderBy(p => p.AnsweredQuestionDate).Select(data => new Query_Dto
                {
                    Id = data.Id,
                    UserID = data.UserID,
                    UserName = _IdentityUserManager.Users.Where(x => x.Id == data.UserID).Select(x => x.UserName).FirstOrDefault(),
                    Question = data.Question,
                    Answer = data.Answer,
                    AnsweredBy = _IdentityUserManager.Users.Where(x => x.Id == data.AnsweredBy).Select(x => x.UserName).FirstOrDefault(),
                    QuestionAskedDate = data.QuestionAskedDate,
                    QuestionDate = data.QuestionAskedDate.ToString("dd/MM/yyyy hh:mm tt"),
                    AnsweredQuestionDate = data.AnsweredQuestionDate,
                    AnswerDate = data.AnsweredQuestionDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt"),
                    Email = _IdentityUserManager.Users.Where(x => x.Id == data.UserID).Select(x => x.Email).FirstOrDefault()
                }).ToList();
            }
            return queries;
        }

        public void insert(Query_Dto query_Dto)// insert question in the database that generated by user.
        {
            Query query = new Query();
            query.UserID = query_Dto.UserID;
            query.Question = query_Dto.Question;
            query.QuestionAskedDate = query_Dto.QuestionAskedDate;
            _context.queries.Add(query);
            _context.SaveChanges();
        }

        public void InsertAnswerToQuestion(Query_Dto query_Dto, string LoginSessionID)
        {
            Query query = _context.queries.Where(p => p.Id == query_Dto.Id).FirstOrDefault();
            query.Answer = query_Dto.Answer;
            query.AnsweredBy = LoginSessionID;
            query.AnsweredQuestionDate = DateTime.Now;
            _context.Entry(query).State = EntityState.Modified;
            _context.SaveChanges();
        }



        public async Task<List<Query_Dto>> QueryListForUserDatatable(DataTable_Dto dataTable_, string LoginSessionID)
        {
            var Records = new List<Query_Dto>();
            Records = await GetAllQueries(LoginSessionID);


            if (!string.IsNullOrEmpty(dataTable_.SearchValue))
            {
                Records = Records.Where(x => x.Question.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Answer.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.AnsweredBy.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.QuestionDate.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.AnswerDate.ToLower().Contains(dataTable_.SearchValue.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(dataTable_.sortColumn) && !string.IsNullOrEmpty(dataTable_.sortColumnDirection))
            {

                IEnumerable<Query_Dto> QueryList = Records;

                switch (dataTable_.sortColumn)
                {

                    case "Question":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.Question) : QueryList.OrderByDescending(u => u.Question);
                        break;

                    case "Answer":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.Answer) : QueryList.OrderByDescending(u => u.Answer);
                        break;

                    case "AnsweredBy":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.AnsweredBy) : QueryList.OrderByDescending(u => u.AnsweredBy);
                        break;

                    case "QuestionDate":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.QuestionDate) : QueryList.OrderByDescending(u => u.QuestionDate);
                        break;
                    case "AnswerDate":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.AnswerDate) : QueryList.OrderByDescending(u => u.AnswerDate);
                        break;

                }



                Records = QueryList.ToList();

            }

            return Records;
        }





        public async Task<List<Query_Dto>> QueryListForCADatatable(DataTable_Dto dataTable_, string LoginSessionID)
        {
            var Records = new List<Query_Dto>();
            Records = await GetAllQueries(LoginSessionID);


            if (!string.IsNullOrEmpty(dataTable_.SearchValue))
            {
                Records = Records.Where(x => x.UserName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Email.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Question.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.QuestionDate.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.AnswerDate.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Answer.ToLower().Contains(dataTable_.SearchValue.ToLower())
                ).ToList();
            }

            if (!string.IsNullOrEmpty(dataTable_.sortColumn) && !string.IsNullOrEmpty(dataTable_.sortColumnDirection))
            {

                IEnumerable<Query_Dto> QueryList = Records;

                switch (dataTable_.sortColumn)
                {

                    case "UserName":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.UserName) : QueryList.OrderByDescending(u => u.UserName);
                        break;

                    case "Email":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.Email) : QueryList.OrderByDescending(u => u.Email);
                        break;

                    case "Question":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.Question) : QueryList.OrderByDescending(u => u.Question);
                        break;

                    case "QuestionDate":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.QuestionDate) : QueryList.OrderByDescending(u => u.QuestionDate);
                        break;
                    case "AnswerDate":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                                QueryList.OrderBy(u => u.AnswerDate) : QueryList.OrderByDescending(u => u.AnswerDate);
                        break;
                    case "Answer":
                        QueryList = dataTable_.sortColumnDirection == "asc" ?
                        QueryList.OrderBy(u => u.Answer) : QueryList.OrderByDescending(u => u.Answer);
                        break;
                }



                Records = QueryList.ToList();

            }

            return Records;
        }



    }
}
