import { useState, useEffect } from 'react';
import './App.css';
import React from 'react';
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
    Created = 0,
    Applied = 1,
    RejectedByCompany = 2,
    RejectedByApplicant = 3,
    InterviewScheduled = 4,
    InterviewedAndAwaitingResponse = 5,
    OfferHasBeenMade = 6,
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

/** Converts dates in the string value return by the server to an actual Date object **/
function dateFromString(date: string) : Date {
    return new Date(date.split("T")[0]);
}

/** Trims date strings from the server to the format needed here **/
function trimDateString(date: string): string {
    return date.split("T")[0];
}



function App() {

    const [userEmail, setUserEmail] = useState("");
    const [userName, setUserName] = useState("");
    const [message, setMessage] = useState("");
    const [jobApplications, setJobApplications] = useState([]);
    const [selectedJob, setSelectedJob] = useState(null);
    const [editedJob, setEditedJob] = useState({
        id: 0,
        companyName: "",
        position: "",
        status: "",
        dateApplied: "",
    });

    const jobStatuses = [
        "Created",
        "Applied",
        "RejectedByCompany",
        "RejectedByApplicant",
        "InterviewScheduled",
        "InterviewedAndAwaitingResponse",
        "OfferHasBeenMade"
    ];

    // Load userEmail from sessionStorage when the component mounts
    useEffect(() => {
        const storedEmail = sessionStorage.getItem("userEmail");
        if (storedEmail) {
            setUserEmail(storedEmail); // Restore the stored value
        }
    }, []);

    // Function to save userEmail to sessionStorage
    const saveFormValues = () => {
        sessionStorage.setItem("userEmail", userEmail);
    };

    // Invoke saveFormValues whenever userEmail changes
    useEffect(() => {
        if (userEmail) {
            saveFormValues();
        }
    }, [userEmail]); // Depend on userEmail, so it runs when userEmail changes

    const isValidEmail = (email: string) => {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    };

    // check email for validity with popup if not valid
    const ensureValidEmail = (email: string): boolean => {
        if (!isValidEmail(email)) {
            alert("Please enter a valid email before proceeding!");
            return false; 
        }
        return true; 
    };

    const handleAddUser = async () => {

        if (!ensureValidEmail(userEmail)) {
            return;
        }

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

        if (!ensureValidEmail(userEmail)) {
            return;
        }

        fetch(`${API_BASE_URL}/api/${userEmail}/JobApplications`, { method: "GET" })
            .then((res) => res.json())
            .then((data) => setJobApplications(data))
            .catch((err) => console.error("Error fetching job applications", err));
    };

    // Fetch job application details by ID
    const fetchJobDetails = async (id) => {

        if (!ensureValidEmail(userEmail)) {
            return;
        }

        fetch(`${API_BASE_URL}/api/${userEmail}/JobApplications/${id}`)
            .then((res) => res.json())
            .then((data) => {
                setSelectedJob(data);
                setEditedJob({
                    id: data.id,
                    companyName: data.companyName,
                    position: data.position,
                    status: data.status,
                    dateApplied: trimDateString(data.dateApplied),
                });
            })
            .catch((err) => console.error("Error fetching job details", err));
    };

    const deleteJob = async (id) => {

        if (!ensureValidEmail(userEmail)) {
            return;
        }

        fetch(`${API_BASE_URL}/api/${userEmail}/JobApplications/Delete/${id}`, {
            method: "DELETE",
        })
            .then((res) => {
                if (res.ok) {
                    setMessage("Application deleted");
                    if (editedJob.id === id) {
                        setSelectedJob(null);
                    }
                    handleGetJobApplications();//update the list
                }
                else {
                    setMessage("Server error, please try again");
                }
            });

    };
    const startNewJobApplication = async () => {

        if (!ensureValidEmail(userEmail)) {
            return;
        }

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

    const handleSaveChanges = () => {

        if (!ensureValidEmail(userEmail)) {
            return;
        }
        const updatedApplication: jobApplication = {
            id: editedJob.id,
            companyName: editedJob.companyName,
            position: editedJob.position,
            status: jobApplicationStatus[editedJob.status],
            dateApplied: dateFromString(editedJob.dateApplied)
        };

        const dbg = JSON.stringify(updatedApplication);

        fetch(`${API_BASE_URL}/api/${userEmail}/JobApplications/Update`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updatedApplication),
        })
            .then((res) => {
                if (res.ok) {
                    setMessage("changes saved");
                }
                else {
                    setMessage("error saving changes");
                }
            })
            .catch((err) => console.error("Error adding job application", err)).
            then(() => {
                handleGetJobApplications();
            });
    };

    return (
        <div className="container">
            <h1>User Management</h1>
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
                            <li key={job.id} style={{
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center',
                                padding: '8px 0',
                                borderBottom: '1px solid #eee'
                            }}>
                                <div style={{ flex: '1' }}>
                                    <span style={{ display: 'inline-block', width: '100px', textAlign: 'right', marginRight: '10px' }}>
                                        <strong>Company:</strong>
                                    </span>
                                    {job.companyName}
                                </div>

                                <div style={{ flex: '1' }}>
                                    <span style={{ display: 'inline-block', width: '100px', textAlign: 'right', marginRight: '10px' }}>
                                        <strong>Position:</strong>
                                    </span>
                                    {job.position}
                                </div>

                                <div style={{ width: '120px', display: 'flex', justifyContent: 'flex-end', gap: '8px' }}>
                                    <button
                                        onClick={() => fetchJobDetails(job.id)}
                                        style={{
                                            padding: '4px 8px',
                                            backgroundColor: '#3B82F6',
                                            color: 'white',
                                            border: 'none',
                                            borderRadius: '4px'
                                        }}
                                    >
                                        View
                                    </button>
                                    <button
                                        onClick={() => deleteJob(job.id)}
                                        style={{
                                            padding: '4px 8px',
                                            backgroundColor: '#EF4444',
                                            color: 'white',
                                            border: 'none',
                                            borderRadius: '4px'
                                        }}
                                    >
                                        Delete
                                    </button>
                                </div>
                            </li>
                        ))}
                    </ul>
                </div>
            )}

            <div className="p-6 max-w-lg mx-auto bg-white rounded-xl shadow-md">
                {/* Job Application Details Form */}
                {selectedJob && (
                    <div className="job-details">
                        <h2>Edit Job Application</h2>

                        {/* Form grid container */}
                        <div style={{
                            display: 'grid',
                            gridTemplateColumns: '1fr 2fr',
                            gap: '10px',
                            alignItems: 'center'
                        }}>
                            <label htmlFor="companyName" style={{ textAlign: 'right' }}>Company name:</label>
                            <input
                                type="text"
                                name="companyName"
                                value={editedJob.companyName}
                                onChange={handleJobChange}
                            />

                            <label htmlFor="position" style={{ textAlign: 'right' }}>Position title:</label>
                            <input
                                type="text"
                                name="position"
                                value={editedJob.position}
                                onChange={handleJobChange}
                            />

                            <label htmlFor="status" style={{ textAlign: 'right' }}>Status:</label>
                            <select name="status" value={editedJob.status} onChange={handleJobChange}>
                                {jobStatuses.map((status) => (
                                    <option key={status} value={status}>
                                        {status}
                                    </option>
                                ))}
                            </select>

                            <label htmlFor="dateApplied" style={{ textAlign: 'right' }}>Date applied:</label>
                            <input
                                type="date"
                                name="dateApplied"
                                value={editedJob.dateApplied}
                                onChange={handleJobChange}
                            />
                        </div>


                        {/* Save Changes button - placed outside the grid but inside the job-details div */}
                        <div style={{
                            marginTop: '20px',
                            display: 'flex',
                            justifyContent: 'flex-end'
                        }}>
                            <button
                                style={{
                                    backgroundColor: '#3B82F6', // blue color
                                    color: 'white',
                                    padding: '8px 16px',
                                    borderRadius: '4px',
                                    border: 'none',
                                    cursor: 'pointer'
                                }}
                                onClick={handleSaveChanges}
                            >
                                Save Changes
                            </button>
                        </div>

                    </div>

                )}
            </div>
        </div>
    );
    
}

export default App;