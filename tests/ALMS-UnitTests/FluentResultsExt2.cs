using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALMS_UnitTests {
    public static class FluentAssert {
        public static void IsFailed(Result res) {
            if (res.IsSuccess) {
                throw new Exception("Result should be failed, is success instead");
            }
        }

        public static void IsFailed<T>(Result<T> res) {
            if (res.IsSuccess) {
                throw new Exception($"Result should have failed, is success instead with"
                    + $"the following data: \n{res.Value}");
            }
        }

        public static void IsSuccess(Result res) {
            if (res.IsFailed) {
                throw new Exception("Result should have been success, was failure instead with" +
                    $"following data:\n{String.Join('\n', res.Errors.Select(err => $"- {err.Message}"))}");
            }
        }

        public static void IsSuccess<T>(Result<T> res) {
            if (res.IsFailed) {
                throw new Exception("Result should have been success, was failure instead with" +
                    $"following data:\n{String.Join('\n',res.Errors.Select(err => $"- {err.Message}"))}");
            }
        }
    }
}
