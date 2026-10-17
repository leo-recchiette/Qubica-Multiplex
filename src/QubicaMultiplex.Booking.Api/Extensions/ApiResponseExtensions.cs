using System.Net;
using QubicaMultiplex.Booking.Api.Exceptions;
using QubicaMultiplex.Booking.Api.Models;
using Refit;

namespace QubicaMultiplex.Booking.Api.Extensions;

public static class ApiResponseExtensions
{
    public static ShowtimeDetails GetShowtimeOrThrow(this IApiResponse<ShowtimeDetails> response, int showId)
    {
        switch (response)
        {
            case { StatusCode: HttpStatusCode.NotFound }:
                throw new ShowNotFoundException(showId);

            case { IsSuccessStatusCode: false }:
                throw response.Error!;
        }

        return response.Content!;
    }

    public static Customer GetCustomerOrThrow(this IApiResponse<Customer> response, int customerId)
    {
        switch (response)
        {
            case { StatusCode: HttpStatusCode.NotFound }:
                throw new CustomerNotFoundException(customerId);

            case { IsSuccessStatusCode: false }:
                throw response.Error!;
        }

        return response.Content!;
    }
}
