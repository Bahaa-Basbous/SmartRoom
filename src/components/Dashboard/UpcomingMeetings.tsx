import { useEffect, useState } from "react";
import axios from "../../services/api";

export default function UpcomingMeetings() {
  const [meetings, setMeetings] = useState([]);

  useEffect(() => {
    axios.get("/Meeting").then((res) => setMeetings(res.data));
  }, []);

  return (
    <div className="bg-white rounded-2xl shadow-md p-6 hover:shadow-lg transition">
      <h2 className="text-xl font-bold text-indigo-600 mb-4 flex items-center gap-2">
        📅 Upcoming Meetings
      </h2>
      {meetings.length === 0 ? (
        <p className="text-gray-500">No upcoming meetings.</p>
      ) : (
        <ul className="space-y-2">
          {meetings.map((m:any) => (
            <li key={m.meetingID} className="border border-indigo-100 p-3 rounded-lg bg-indigo-50">
              <div className="font-semibold text-indigo-700">{m.title}</div>
              <div className="text-sm text-gray-500">
                {new Date(m.dateTime).toLocaleString()}
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}