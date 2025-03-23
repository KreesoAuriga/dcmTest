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
    const [jobApplications, setJobApplications] = useState([]);
    const [selectedJob, setSelectedJob] = useState(null);
    const [editedJob, setEditedJob] = useState({
        companyName: "",
        position: "",
        status: "",
        dateApplied: "",
    });
    //const [newJobApplicationId, setNewJobApplicationId] = useState("");

    const jobStatuses = [
        "Created",
        "Applied",
        "RejectedByCompany",
        "RejectedByApplicant",
        "InterviewScheduled",
        "InterviewedAndAwaitingResponse",
        "OfferHasBeenMade"
    ];

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

    // Fetch job applications when userEmail is set
    const handleGetJobApplications = async () => {
        fetch(`${API_BASE_URL}/api/${userEmail}/JobApplications`, { method: "GET" })
            .then((res) => res.json())
            .then((data) => setJobApplications(data))
            .catch((err) => console.error("Error fetching job applications", err));
    };

    // Fetch job application details by ID
    const fetchJobDetails = async (id) => {
        fetch(`${API_BASE_URL}/api/${userEmail}/JobApplications/${id}`)
            .then((res) => res.json())
            .then((data) => {
                setSelectedJob(data);
                setEditedJob({
                    companyName: data.companyName,
                    position: data.position,
                    status: data.status,
                    dateApplied: data.dateApplied,
                });
            })
            .catch((err) => console.error("Error fetching job details", err));
    };
    const startNewJobApplication = async () => {
        const emptyApplication: newJobApplication = {
            companyName: "",
            position: "",
            dateApplied: new Date()
        };

        fetch(`${API_BASE_URL}/api/${userEmail}/JobApplications/Add`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(emptyApplication),
        })
        .then((res) => res.json()) // Assuming the response is just the job ID
        .then((data) => {
            const newJobId = data; // The returned data is the job ID
            console.log("New job application ID:", newJobId);

            // After adding the job, fetch its details using the ID
            fetchJobDetails(newJobId);
        })
        .catch((err) => console.error("Error adding job application", err));
    };

    // Handle changes in the editable job fields
    const handleJobChange = (e) => {
        setEditedJob({ ...editedJob, [e.target.name]: e.target.value });
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
            <button onClick={handleAddUser}>Delete User</button>
            <button onClick={handleGetJobApplications}>Get job applications</button>
            <button onClick={startNewJobApplication}>Start new job application</button>

            {message && <p>{message}</p>}

            {/* Job Applications List */}
            {jobApplications.length > 0 && (
                <div className="job-applications">
                    <h2>Job Applications</h2>
                    <ul>
                        {jobApplications.map((job) => (
                            <li key={job.id}>
                                {job.companyName} - {job.position}
                                <button onClick={() => fetchJobDetails(job.id)}>View</button>
                            </li>
                        ))}
                    </ul>
                </div>
            )}

            {/* Job Application Details Form */}
            {selectedJob && (
                <div className="job-details">
                    <h2>Edit Job Application</h2>
                    <label htmlFor="companyName">Company name:</label>
                    <input
                        type="text"
                        name="companyName"
                        value={editedJob.companyName}
                        onChange={handleJobChange}
                    />
                    <br />
                    <label htmlFor="position">Position title:</label>
                    <input
                        type="text"
                        name="position"
                        value={editedJob.position}
                        onChange={handleJobChange}
                    />
                    <br />
                    <label htmlFor="status">Status:</label>
                    <select name="status" value={editedJob.status} onChange={handleJobChange}>
                        {jobStatuses.map((status) => (
                            <option key={status} value={status}>
                                {status}
                            </option>
                        ))}
                    </select>
                    <br />
                    <label htmlFor="dateApplied">Date applied:</label>
                    <input
                        type="date"
                        name="dateApplied"
                        value={editedJob.dateApplied}
                        onChange={handleJobChange}
                    />
                </div>
            )}
        </div>
    );
    
}

export default App;