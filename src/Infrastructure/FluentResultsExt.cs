using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FailureException : Exception {
    List<IError> errors;

    public FailureException(List<IError> errors) {
        this.errors = errors;
    }
}

public static class FluentResultsExt {
    public static void Unwrap(this Result result) {
        if (result.IsFailed) {
            // foreach (var error in result.Errors)
            // {
            //     PlmLogger.Log(error.Message);
            // }
            throw new FailureException(result.Errors);
        }
    }

    public static T Unwrap<T>(this Result<T> result) {
        if (result.IsFailed) {
            // foreach (var error in result.Errors)
            // {
            //     PlmLogger.Log(error.Message);
            // }
            throw new FailureException(result.Errors);
        }

        return result.Value;
    }

    public static Result<T> OnError<T>(this Result<T> result, Action<IError> action) {
        if (result.IsFailed) {
            foreach (var error in result.Errors) {
                action(error);
            }
        }
        return result;
    }
}
