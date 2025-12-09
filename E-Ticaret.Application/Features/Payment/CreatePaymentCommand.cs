using E_Ticaret.Domain.Common;
using E_Ticaret.Infrastructure.Abstractions;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Payment;
    public sealed  record CreatePaymentCommand(PaymentRequestDto PaymentRequestDto) :IRequest<Result<CreatePaymentResultDto>>;


internal sealed class CreatePaymentCommandHandler(IPaymentService paymentService) : IRequestHandler<CreatePaymentCommand, Result<CreatePaymentResultDto>>
{
    public async Task<Result<CreatePaymentResultDto>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        return await paymentService.ProcessPaymentAsync(request.PaymentRequestDto, cancellationToken);
    }
}