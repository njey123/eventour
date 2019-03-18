# Import flask microframework library
from flask import Flask, request
import json
 
# Initialize the flask application
app = Flask(__name__)

# Default Flask local web server: http://127.0.0.1:5000/

# Endpoint with post method
@app.route("/api/v1.0/csharp_python_restfulapi_json", methods=["POST"])
def csharp_python_restfulapi_json():
    """
    simple c# test to call python restful api web service
    """
    try:                
#         get request json object
        request_json = request.get_json()      
#         convert to response json object 
        response = jsonify(request_json)
        response.status_code = 200  
    except:
        exception_message = sys.exc_info()[1]
        response = json.dumps({"content":exception_message})
        response.status_code = 400
    return response

@app.route("/test", methods=["GET", "POST"])
def test_route():
    # Get data from client
    dest = request.form.get('dest')
    start_date = request.form.get('start_date')
    end_date = request.form.get('end_date')

    # Print to console what Xamarin client sent
    print("XAMARIN SENT US TO {}, FROM {} TO {}".format(dest, start_date, end_date))

    # Crawl every 2 to 4 weeks
    # Store data in database
    # Retrieve data in database and form response

    # Send response from server to client
    response_json = {
        'Dest': dest,
        'StartDate': start_date,
        'EndDate': end_date,
        'Attractions': ["Eiffel Tower"],
        'Ratings': ["4.2"] 
    }
    return json.dumps(response_json)

if __name__ == "__main__":
#     Run flask application in debug mode
    app.run(debug=True)  # Make sure to change debug=True to False in a production environment
