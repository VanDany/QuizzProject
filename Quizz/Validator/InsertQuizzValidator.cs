using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Routing.Constraints;
using Quizz.Context;
using Quizz.Models;

namespace Quizz.Validator
{
    public class InsertQuizzValidator : AbstractValidator<modQuizz>
    {
        public InsertQuizzValidator()
        {
            RuleFor(x => x.Title)
                .NotNull().WithMessage("Vous devez donner un titre au Quizz")
                .Length(5, 30).WithMessage("La longueur du titre doit être comprise entre 5 et 30 caractères.");
            RuleFor(x => x.Category)
                .NotNull().WithMessage("Vous devez indiquer une catégorie")
                .Length(5, 30)
                .WithMessage("La longueur du nom de la catégorie doit être comprise entre 5 et 30 caractères.");
            RuleFor(x => x.Difficulty)
                .NotNull().WithMessage("Vous devez indiquer un niveau de difficulté.");
        }
    }
}
