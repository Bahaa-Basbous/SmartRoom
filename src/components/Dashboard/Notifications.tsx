import { useEffect, useState } from "react";
import axios from "../../services/api";

export default function Notifications() {
  const [notifications, setNotifications] = useState([]);

  useEffect(() => {
    axios.get("/Notification").then((res) => setNotifications(res.data));
  }, []);

  return (
    <div className="bg-white rounded-2xl shadow-md p-6 hover:shadow-lg transition">
      <h2 className="text-xl font-bold text-yellow-600 mb-4 flex items-center gap-2">
        🔔 Notifications
      </h2>
      {notifications.length === 0 ? (
        <p className="text-gray-500">You're all caught up!</p>
      ) : (
        <ul className="space-y-2">
          {notifications.map((n:any) => (
            <li key={n.id} className="border border-yellow-100 p-3 rounded-lg bg-yellow-50">
              <div className="font-semibold text-yellow-700">{n.message}</div>
              <div className="text-sm text-gray-500">
                {new Date(n.timestamp).toLocaleString()}
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
