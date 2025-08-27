using Microsoft.Extensions.Configuration;
using tourism_api.Domain;
using tourism_api.Repositories;

namespace tourism_api.Service
{
    public class tourService
    {
        private readonly TourRepository _tourRepo;

        public tourService(IConfiguration configuration)
        {
            _tourRepo = new TourRepository(configuration);
        }

        public TourStats GetByUserAndDateRange(int guideId, DateTime startDate, DateTime endDate)
        {
            List<Tour> tours = _tourRepo.GetAllByGuideAndDate(guideId, startDate, endDate);

            var tourStats = new TourStats
            {
                tourMaxRes = tours
                    .OrderByDescending(t => t.TourReservations.Count)
                    .Take(5)
                    .ToList(),

                tourMinRes = tours
                    .OrderBy(t => t.TourReservations.Count)
                    .Take(5)
                    .ToList(),

                tourMaxPercentage = tours
                    .Where(t => t.MaxGuests > 0)
                    .OrderByDescending(t => t.TourReservations.Count / (double)t.MaxGuests)
                    .Take(5)
                    .ToList(),

                tourMinPercentage = tours
                    .Where(t => t.MaxGuests > 0)
                    .OrderBy(t => t.TourReservations.Count / (double)t.MaxGuests)
                    .Take(5)
                    .ToList(),

                totalTours = tours.Count
            };

            return tourStats;
        }
        public bool CloneKeypointsFromOldToNew(int oldTourId, int newTourId)
        {
            Tour oldTour = _tourRepo.GetById(oldTourId);
            Tour newTour = _tourRepo.GetById(newTourId);

            if (oldTour == null || newTour == null)
                return false;

            foreach (var keyPoint in oldTour.KeyPoints)
            {
                // Associate new keypoint with new tour
                bool assigned = _tourRepo.AddKeypointTour(newTourId, keyPoint.Id);
                if (!assigned)
                    return false;
            }

            return true;
        }


    }
}
