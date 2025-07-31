import { useEffect, useState } from "react";
import axios from "../../services/api";

export default function RecentMoMs() {
  const [moms, setMoms] = useState([]);

  useEffect(() => {
    axios.get("/MoM").then((res) => setMoms(res.data));
  }, []);

  return (
    <div className="bg-white rounded-2xl shadow-md p-6 hover:shadow-lg transition">
      <h2 className="text-xl font-bold text-purple-600 mb-4 flex items-center gap-2">
        📝 Recent Meeting Notes
      </h2>
      {moms.length === 0 ? (
        <p className="text-gray-500">No meeting notes available.</p>
      ) : (
        <ul className="space-y-2">
          {moms.map((m:any) => (
            <li key={m.moMID} className="border border-purple-100 p-3 rounded-lg bg-purple-50">
              <div className="font-semibold text-purple-700">{m.summary}</div>
              <div className="text-sm text-gray-500">
                {new Date(m.createdAt).toLocaleDateString()}
              </div>
            </li>
          ))}
        </ul>
      )}
              <div className="text-right mt-4">
          <a
            href="/mom/view"
            className="text-sm text-purple-700 hover:underline"
          >
            View all ➜
          </a>
        </div>

    </div>
    
  );
}

