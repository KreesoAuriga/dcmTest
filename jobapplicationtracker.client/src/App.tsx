import React from "react";
import { useEffect, useState } from 'react';
import './App.css';
/*
interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
*/
const API_BASE_URL = window.location.protocol === "https:"
    ? "https://localhost:7056"  // Use HTTPS if frontend is HTTPS
    : "http://localhost:5014";   // Use HTTP otherwise

enum jobApplicationStatus {
    Created,
    Applied,
    RejectedByCompany,
    RejectedByApplicant,
    InterviewScheduled,
    InterviewedAndAwaitingResponse,
    OfferHasBeenMade,
}

interface newJobApplication {
    companyName: string;
    position: string;
    dateApplied: Date;
}

interface jobApplication extends newJobApplication {
    id: number;
    status: jobApplicationStatus;
}

interface newUser {
    email: string;
    userName: string;
}

interface user extends newUser {
    jobApplications: jobApplication[];
}




function App() {

    const [userEmail, setUserEmail] = useState("");
    const [userName, setUserName] = useState("");
    const [message, setMessage] = useState("");

    const handleAddUser = async () => {
        const user: newUser = { email: userEmail, userName: userName };


        const response = await fetch(`${API_BASE_URL}/api/User`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(user),
        });

        if (response.ok) {
            setMessage(`User ${userEmail} added successfully.`);
        } else {
            setMessage("Error adding user.");
        }
    };

    return (
        <div className="container">
            <h1>User Management</h1>
            <input
                type="text"
                placeholder="Enter user name"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
            />
            <input
                type="text"
                placeholder="Enter user email"
                value={userEmail}
                onChange={(e) => setUserEmail(e.target.value)}
            />
            <button onClick={handleAddUser}>Add User</button>

            {message && <p>{message}</p>}
        </div>
    );
    /*
    const [forecasts, setForecasts] = useState<Forecast[]>();
    
    useEffect(() => {
        populateWeatherData();
    }, []);



    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel">Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );

    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        setForecasts(data);
    }
    */
}

export default App;