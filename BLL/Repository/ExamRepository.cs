﻿using BLL.IRepository;
using BLL.ViewModels;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class ExamRepository:IExamRepository
    {

        private readonly ITIContext context;
        public ExamRepository(ITIContext _context)
        {
            context = _context;
        }
        public Exam GetExamById(int ExamId)
        {
            var exam = context.Exams.Include(e=>e.QIds).Include(e => e.Crs).FirstOrDefault(e => e.ExId == ExamId);
            return exam;
        }

        public Exam GetCurrentExamByStudentId(int StudentId)
        {
            var currentExam = GetAllExamByStudentId(StudentId).FirstOrDefault(e => e.Date == DateOnly.FromDateTime(DateTime.Today));
           
            return currentExam;
        }

        public List<Exam> GetAllExamByStudentId(int StudentId)
        {
            var stdCourses = context.StudentCourses.Include(c=>c.Crs).Where(c => c.SId == StudentId).ToList();
            List<Exam> exams = new List<Exam>();
            foreach (var course in stdCourses)
            {
                var exam = context.Exams.FirstOrDefault(e => e.CrsId == course.CrsId);
                if (exam!=null)
                {
                    exams.Add(exam);
                }
            }
            return exams;
        }

        public List<Question> GetQuestionsByExamId(int ExamId)
        {
            var exam = GetExamById(ExamId);
            var examQuestions = exam.QIds.ToList();
            for (int i=0; i<examQuestions.Count;i++)
            {
                examQuestions[i] = context.Questions.Include(q => q.Ches).FirstOrDefault(s => s.QId == examQuestions[i].QId);
            }
            return examQuestions;
        }

        public List<Choice> GetChoicesByQuestionId(int QuestId)
        {
            var Question = context.Questions.Include(q=>q.Ches).FirstOrDefault(s=>s.QId== QuestId);
            var QuestChoices = Question.Ches.ToList();
            return QuestChoices;
        }
        public Status SubmitStudentExam(StudentExamViewModel studentExamViewModel)
        {
            if (studentExamViewModel.QuestionsAnswers.Count>0)
            {
                foreach (var qAnswer in studentExamViewModel.QuestionsAnswers)
                {
                    var stdExam = new StdExam();
                    stdExam.StdId = studentExamViewModel.StdId;
                    stdExam.ExId = studentExamViewModel.ExamId;
                    stdExam.QId = qAnswer.Key;
                    stdExam.StdAnswer = qAnswer.Value;
                    context.StdExams.Add(stdExam);
                }
                context.SaveChanges();
                return Status.Success;
            }
            return Status.Failed;
        }
        public Tuple<int,int> CalculateTotalGrade(StudentExamViewModel studentExamViewModel)
        {
         
            if (studentExamViewModel.QuestionsAnswers.Count > 0)
            {
                int totalGrade = 0;
                int overAllGrade = 0;
                foreach (var qAnswer in studentExamViewModel.QuestionsAnswers)
                {
                    var question = context.Questions.FirstOrDefault(q => q.QId == qAnswer.Key);
                    var questionModelAns = question.Answer;
                    overAllGrade += question.Score.Value;
                    if (questionModelAns == qAnswer.Value )
                    {
                        totalGrade += question.Score.Value;
                    }
                }
                SaveExamTotalScore(studentExamViewModel.ExamId, totalGrade);
                //context.SaveChanges();
                return new Tuple<int,int >(totalGrade,overAllGrade);
            }
            return new Tuple<int, int > (0, 0);
        }
        private void SaveExamTotalScore(int ExamId,int TotalScore)
        {
            var exam = GetExamById(ExamId);
            exam.TotalScore = TotalScore;
            context.SaveChanges();
        }
        public bool IsStudentExamSubmitted(int StudentId, int ExamId)
        {
            var stdExam = context.StdExams.FirstOrDefault(e => e.StdId == StudentId && e.ExId == ExamId);
            if (stdExam != null)
            {
                return true;
            }
            return false;
        }
        public bool IsExamTimeUp(int ExamId)
        {
            var exam = GetExamById(ExamId);
            if (exam.Date < DateOnly.FromDateTime(DateTime.Today))
            {
                return true;
            }
            return false;
        }

       

    }
}
