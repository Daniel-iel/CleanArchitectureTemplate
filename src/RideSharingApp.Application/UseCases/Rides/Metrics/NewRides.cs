using System.Diagnostics.Metrics;

namespace RideSharingApp.Application.UseCases.Rides.Metrics;

public interface INewRides
{
    void Increment();
    //void RecordRideDuration(double durationInSeconds);
}

internal class NewRides : INewRides
{
    private readonly Counter<long> _ordersCounter;
    private readonly Histogram<double> _rideDurationHistogram;

    public NewRides(Meter meter)
    {
        _ordersCounter = meter.CreateCounter<long>(
            "ride_total",
            description: "Quantidade total de corridas"
        );
        //_rideDurationHistogram = meter.CreateHistogram<double>(
        //    "ride_duration_seconds",
        //    unit: "s",
        //    description: "Duração das corridas em segundos"
        //);
    }

    public void Increment()
    {
        _ordersCounter.Add(1);
    }

    //public void RecordRideDuration(double durationInSeconds)
    //{
    //    _rideDurationHistogram.Record(durationInSeconds);
    //}
}
