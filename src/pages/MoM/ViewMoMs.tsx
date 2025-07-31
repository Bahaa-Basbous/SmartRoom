import { useEffect, useState } from "react";
import axios from "../../services/api";

export default function ViewMoMs() {
  const [moms, setMoms] = useState<any[]>([]);

  useEffect(() => {
    axios.get("/mom").then((res) => setMoms(res.data));
  }, []);

  const handleGeneratePDF = async (momId: number) => {
    try {
      const response = await axios.get(`/mom/${momId}/generate-pdf`, {
        responseType: "blob",
      });

      const blob = new Blob([response.data], { type: "application/pdf" });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.download = `MoM_${momId}.pdf`;
      link.click();
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error("Failed to generate PDF:", error);
      alert("Failed to generate PDF.");
    }
  };

  return (
    <div className="max-w-5xl mx-auto mt-12 px-4">
      <h2 className="text-4xl font-extrabold mb-8 text-indigo-700 text-center">
        📋 Minutes of Meetings
      </h2>

      {moms.length === 0 ? (
        <div className="text-center text-gray-500">No MoMs available.</div>
      ) : (
        <div className="grid gap-6">
          {moms.map((mom) => (
            <div
              key={mom.moMID}
              className="bg-gradient-to-r from-cyan-50 to-blue-50 rounded-2xl shadow-xl p-6 border border-blue-200 hover:shadow-2xl transition-all space-y-3"
            >
              <h3 className="text-2xl font-bold text-blue-800">
                📌 Meeting:{" "}
                <span className="font-semibold text-blue-600">
                  {mom.meetingTitle || "N/A"}
                </span>
              </h3>

              <p>
                <strong>Summary:</strong> {mom.summary}
              </p>
              <p>
                <strong>Notes:</strong> {mom.notes}
              </p>
              <p className="text-sm text-gray-600">
                🕒 <strong>Created At:</strong>{" "}
                {new Date(mom.createdAt).toLocaleString()}
              </p>
              <p className="text-sm text-gray-600">
                👤 <strong>Created By:</strong>{" "}
                {mom.createdByName || "Unknown"}
              </p>

              {mom.filePath && (
                <a
                  href={mom.filePath}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="inline-block mt-3 bg-indigo-100 text-indigo-800 text-sm font-medium px-4 py-1 rounded hover:bg-indigo-200 transition"
                >
                  📎 View Attachment
                </a>
              )}

              <button
                onClick={() => handleGeneratePDF(mom.moMID)}
                className="mt-3 bg-purple-600 text-white text-sm font-medium px-4 py-1 rounded hover:bg-purple-700 transition"
              >
                📄 Download PDF
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
