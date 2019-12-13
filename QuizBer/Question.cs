 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBer
{
	public abstract class Question
	{
		public abstract string QuestionTypeDisplay { get; }
		public string QuestionString { get; set; }
		public int NumRequiredAnswers { get; set; } = 1;
		public abstract bool AttemptAnswer(string[] attemptedAnswer);
		public virtual List<string> DisplayQuestion()
		{
			return new List<string>()
			{
				QuestionString,
			};
		}
	}

	public class TrueFalseQuestion : Question
	{
		public override string QuestionTypeDisplay { get; } = "True or False Question";
		public bool CorrectAnswerBool { get; set; }
		public override bool AttemptAnswer(string[] attemptedAnswer)
		{
			if (attemptedAnswer[0].ToLower() == CorrectAnswerBool.ToString().ToLower())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class FillInTheBlankQuestion : Question
	{
		public override string QuestionTypeDisplay { get; } = "Fill in the Blank Question";
		public string CorrectAnswerString { get; set; }
		public override bool AttemptAnswer(string[] attemptedAnswer)
		{
			if (attemptedAnswer[0] == CorrectAnswerString)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class OrderQuestion : Question
	{
		private class OrderQuestionAnswer
		{
			public int CorrectOrder { get; set; }
			public int RandomOrder { get; set; }
			public string AnswerString { get; set; }
		}

		private List<OrderQuestionAnswer> OrderQuestionAnswers { get; set; }
		public override string QuestionTypeDisplay { get; } = " Order Question ";

		public OrderQuestion(string[] answersInOrder)
		{
			OrderQuestionAnswers = new List<OrderQuestionAnswer>();
			var rand = new Random();
			for (var i = 0; i < answersInOrder.Length; i++)
			{
				var newOrderQuestionAnswer = new OrderQuestionAnswer()
				{
					AnswerString = answersInOrder[i],
					CorrectOrder = i + 1,
					RandomOrder = rand.Next()
				};
				OrderQuestionAnswers.Add(newOrderQuestionAnswer);
			}
			OrderQuestionAnswers = OrderQuestionAnswers.OrderBy(oqa => oqa.RandomOrder).ToList();
			for (int i = 0; i < OrderQuestionAnswers.Count(); i++)
			{
				OrderQuestionAnswers[i].RandomOrder = i + 1;
			}

			NumRequiredAnswers = OrderQuestionAnswers.Count();
		}
		public override bool AttemptAnswer(string[] attemptedAnswers)
		{
			for (var i = 0; i < attemptedAnswers.Length; i++)
			{
				var matchingOrderQuestionAnswer = OrderQuestionAnswers[int.Parse(attemptedAnswers[i]) - 1];

				if (matchingOrderQuestionAnswer.CorrectOrder - 1 != i)
				{
					return false;
				}
			}
			return true;
		}

		public override List<string> DisplayQuestion()
		{
			var returnList = new List<string>()
			{
				QuestionString,
				string.Empty
			};

			foreach (var orderQuestionAnswer in OrderQuestionAnswers)
			{
				returnList.Add(orderQuestionAnswer.RandomOrder + ". " + orderQuestionAnswer.AnswerString);
			}

			return returnList;
		}

	}

	public class MultipleChoiceQuestion : Question
	{
		public override string QuestionTypeDisplay { get; } = "Multiple Choice Question";

		public string CorrectAnswerString { get; set; }

		public List<string> PossibleAnswers { get; set; }

		public int CorrectAnswerIndex { get; set; }

		public override List<string> DisplayQuestion()
		{
			var returnList = new List<string>()
			{
				QuestionString,
				string.Empty

			};

			for(int i = 0; i < PossibleAnswers.Count; i++)
			{
				returnList.Add((i+1) + ". " + PossibleAnswers[i]);
			}

			return returnList;
		}

		public override bool AttemptAnswer(string[] attemptedAnswer)
		{
			if (int.Parse(attemptedAnswer[0]) - 1 == CorrectAnswerIndex)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}


}
