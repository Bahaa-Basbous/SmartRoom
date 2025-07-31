import { useEffect, useState } from "react";
import axios from "../services/api";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

interface Booking {
  bookingId: number;
  room: { name: string };
  user: { name: string };
  startTime: string;
  endTime: string;
  status: string;
}

export default function PendingBookings() {
  const [pendingBookings, setPendingBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);
  const [processingId, setProcessingId] = useState<number | null>(null);

  const fetchPendingBookings = async () => {
    try {
      const res = await axios.get("/Booking/pending");
      setPendingBookings(res.data);
    } catch (error) {
      toast.error("Failed to load pending bookings.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPendingBookings();
  }, []);

  const updateBookingStatus = async (id: number, status: string) => {
    setProcessingId(id);
    try {
      await axios.put(`/Booking/${id}/status`, { status });
      toast.success(`Booking ${status.toLowerCase()} successfully.`);
      fetchPendingBookings();
    } catch (error: any) {
      toast.error(error.response?.data?.message || "Failed to update booking status.");
    } finally {
      setProcessingId(null);
    }
  };

  const navigate = useNavigate();

  if (loading) return <p>Loading pending bookings...</p>;

  if (pendingBookings.length === 0)
    return (
      <div className="max-w-4xl mx-auto mt-10 p-6">
        <button
          onClick={() => navigate("/dashboard")}
          className="mb-6 inline-flex items-center px-4 py-2 bg-gray-200 rounded-md shadow hover:bg-gray-300 transition"
        >
          ← Back
        </button>
        <p className="text-center text-gray-600 text-lg">No pending bookings.</p>
      </div>
    );

  return (
    <div className="max-w-6xl mx-auto mt-10 p-6">
      <button
        onClick={() => navigate("/dashboard")}
        className="mb-6 inline-flex items-center px-4 py-2 bg-gray-200 rounded-md shadow hover:bg-gray-300 transition"
      >
        ← Back
      </button>
      <h2 className="text-2xl font-semibold mb-6">Pending Bookings</h2>
      <table className="min-w-full table-auto border text-sm">
        <thead>
          <tr className="bg-gray-200">
            <th className="p-2 border">Room</th>
            <th className="p-2 border">User</th>
            <th className="p-2 border">Start Time</th>
            <th className="p-2 border">End Time</th>
            <th className="p-2 border">Actions</th>
          </tr>
        </thead>
        <tbody>
          {pendingBookings.map((booking) => (
            <tr key={booking.bookingId}>
              <td className="p-2 border">{booking.room?.name}</td>
              <td className="p-2 border">{booking.user?.name}</td>
              <td className="p-2 border">{new Date(booking.startTime).toLocaleString()}</td>
              <td className="p-2 border">{new Date(booking.endTime).toLocaleString()}</td>
              <td className="p-2 border space-x-2">
                <button
                  disabled={processingId === booking.bookingId}
                  onClick={() => updateBookingStatus(booking.bookingId, "Approved")}
                  className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700 disabled:opacity-50"
                >
                  Approve
                </button>
                <button
                  disabled={processingId === booking.bookingId}
                  onClick={() => updateBookingStatus(booking.bookingId, "Rejected")}
                  className="bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700 disabled:opacity-50"
                >
                  Reject
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
