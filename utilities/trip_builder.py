"""
Suggested duration - integer state:
0 --> < 1 hour
1 --> 1-2 hours
2 --> 2-3 hours
3 --> More than 3 hours
"""

# Build a trip from scratch ("Suggest A Trip")
# Assumes no attractions in database are repeated and duration is stored as a string in the database
def suggest_trip(num_days, attraction_db_results, rating_db_results, review_count_db_results, image_url_db_results, duration_db_results):
    
    max_hours = 6  # Max number of hours dedicated for attractions per day
    default_hours = 2  # Use if suggested duration is not available for an attraction

    attractions = []  # Use to keep track of which attractions are planned for one day
    ratings = []
    review_counts = []
    image_urls = []
    durations = []

    # Create list of lists of length number of days in the trip
    for day in range(0, num_days):
        attractions.append([])
        ratings.append([])
        review_counts.append([])
        image_urls.append([])
        durations.append([])

    i = 0
    areThereAttractionsLeft = True
    for day in range(0, num_days):
        curr_hours = 0  # Use to keep track of number of hours planned so far for one day

        # If no more attractions left
        if areThereAttractionsLeft == False or i >= len(duration_db_results):
            attractions[day].append('')
            ratings[day].append('')
            review_counts[day].append('')
            image_urls[day].append('')
            durations[day].append('')
            continue

        # If not an empty string
        if duration_db_results[i]:
            curr_hours = curr_hours + int(duration_db_results[i]) + 1  # Add 1 because of the way duration is stored as an integer state
            durations[day].append(duration_db_results[i])
        else:
            curr_hours = curr_hours + default_hours
            durations[day].append(str(default_hours))

        # Loop while number of hours planned for one day is less than the max allowed for one day
        while True:
            # print('Current day: ' + str(day) + '\t\t Curr hours: ' + str(curr_hours))           

            # Store which attractions will be visited for current day in trip
            attractions[day].append(attraction_db_results[i])
            ratings[day].append(rating_db_results[i])
            review_counts[day].append(review_count_db_results[i])
            image_urls[day].append(image_url_db_results[i])

            # Increment iterator
            i = i + 1

            # If no more attractions in database results
            if i >= len(duration_db_results):
                areThereAttractionsLeft = False
                break

            if duration_db_results[i]:
                curr_hours = curr_hours + int(duration_db_results[i]) + 1
            else:
                curr_hours = curr_hours + default_hours

            # If adding another attraction to the day would exceed max number of hours of visiting attractions per day
            if curr_hours > max_hours:
                break
            else:
                if duration_db_results[i]:
                    durations[day].append(duration_db_results[i])
                else:
                    durations[day].append(str(default_hours))

    return attractions, ratings, review_counts, image_urls, durations


# if __name__ == "__main__" and __package__ is None:
#     __package__ = "trip_builder"
