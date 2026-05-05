using Shared.DTOs.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared.CommonResult
{
    public class Result
    {
        protected readonly List<Error> _errors = [];

        //IsSuccess
        public bool IsSuccess => _errors.Count == 0;

        //IsFailure
        public bool IsFailure => !IsSuccess;

        //Errors[Code , Descroption, ErrorType]
        public IReadOnlyList<Error> Errors => _errors;
        //ok
        protected Result()
        {

        }

        // Failure With One Error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        // Failure With Multiple Errors
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        // Static Factory Methods for Ok
        public static Result Ok()
        {
            return new Result();
        }
        // Static Factory Methods for Failure With One Error
        public static Result Failure(Error error)
        {
            return new Result(error);
        }
        // Static Factory Methods for Failure With Multiple Errors
        public static Result Failure(List<Error> errors)
        {
            return new Result(errors);
        }

        public static Result<IEnumerable<AppointmentResponseDTO>> Fail(Error error)
        {
            throw new NotImplementedException();
        }
        public static implicit operator Result(Error error)
        {
            return Failure(error);
        }

        public static implicit operator Result(List<Error> errors)
        {
            return Failure(errors);
        }
    }

    public class Result<TValue> : Result
    {

        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");


        private Result(TValue Value) : base()
        {
            _value = Value;
        }

        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }



        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);

        public static new Result<TValue> Failure(Error error) => new Result<TValue>(error);

        public static new Result<TValue> Failure(List<Error> errors) => new Result<TValue>(errors);

        public static implicit operator Result<TValue>(TValue Value) => Ok(Value);
        public static implicit operator Result<TValue>(Error error) => Failure(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Failure(errors);

    }
}
