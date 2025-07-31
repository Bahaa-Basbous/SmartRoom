import { useState, useEffect } from "react";
import axios from "../../services/api";
import { useNavigate } from "react-router-dom";

export default function CreateMoM() {
  const [meetingId, setMeetingId] = useState("");
  const [summary, setSummary] = useState("");
  const [notes, setNotes] = useState("");
  const [file, setFile] = useState<File | null>(null);
  const [meetings, setMeetings] = useState<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios.get("/Meeting").then((res) => {
      setMeetings(res.data);
    });
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      let uploadedFilePath = "";

      if (file) {
        const formData = new FormData();
        formData.append("file", file);

        const uploadRes = await axios.post("/mom/upload", formData, {
          headers: { "Content-Type": "multipart/form-data" },
        });

        uploadedFilePath = uploadRes.data.filePath;
      }

      await axios.post("/mom", {
        MeetingID: parseInt(meetingId),
        Summary: summary,
        Notes: notes,
        FilePath: uploadedFilePath || null,
      });

      alert("MoM created successfully!");
      navigate("/mom/view");
    } catch (err) {
      console.error(err);
      alert("Failed to create MoM.");
    }
  };

  return (
    <div className="max-w-xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
      <h2 className="text-2xl font-bold mb-4">Create Minutes of Meeting</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <select
          value={meetingId}
          onChange={(e) => setMeetingId(e.target.value)}
          required
          className="w-full p-2 border rounded"
        >
          <option value="">Select Meeting</option>
          {meetings.map((meeting) => (
            <option key={meeting.meetingID} value={meeting.meetingID}>
              {meeting.title}
            </option>
          ))}
        </select>

        <textarea
          placeholder="Summary"
          className="w-full p-2 border rounded"
          value={summary}
          onChange={(e) => setSummary(e.target.value)}
          required
        />
        <textarea
          placeholder="Notes"
          className="w-full p-2 border rounded"
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          required
        />
        

        <button className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
          Submit
        </button>
      </form>
    </div>
  );
}
