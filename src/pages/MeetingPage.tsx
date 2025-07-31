import { useEffect, useState } from "react";
import axios from "../services/api";
import NavBar from "../components/NavBar";
import { CalendarCheck, AlertCircle } from "lucide-react";

export default function MeetingPage() {
  const [title, setTitle] = useState("");
  const [agenda, setAgenda] = useState("");
  const [bookingID, setBookingID] = useState("");
  const [bookings, setBookings] = useState<any[]>([]);
  const [dateTime, setDateTime] = useState("");
  const [error, setError] = useState("");

  const userId = localStorage.getItem("userId");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [bookingsRes, meetingsRes] = await Promise.all([
          axios.get("/Booking"),
          axios.get("/Meeting"),
        ]);

        const approvedBookings = bookingsRes.data.filter(
          (booking: any) => booking.status === "Approved"
        );

        const usedBookingIds = meetingsRes.data.map(
          (meeting: any) => meeting.bookingID
        );

        const availableBookings = approvedBookings.filter(
          (booking: any) => !usedBookingIds.includes(booking.bookingId)
        );

        setBookings(availableBookings);
      } catch (err) {
        console.error("Error fetching data", err);
      }
    };

    fetchData();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const selectedBooking = bookings.find(
      (b) => b.bookingId === parseInt(bookingID)
    );

    if (!selectedBooking) {
      setError("Selected booking is invalid or has been rejected.");
      return;
    }

    const meetingTime = new Date(dateTime).getTime();
    const start = new Date(selectedBooking.startTime).getTime();
    const end = new Date(selectedBooking.endTime).getTime();

    if (meetingTime < start || meetingTime > end) {
      setError("Meeting time must be within the selected booking's time slot.");
      return;
    }

    setError("");

    try {
      await axios.post("/Meeting", {
        title,
        agenda,
        bookingID: parseInt(bookingID),
        organizerID: parseInt(userId!),
        dateTime,
      });

      setTitle("");
      setAgenda("");
      setBookingID("");
      setDateTime("");
      alert("🎉 Meeting scheduled successfully!");
    } catch (err: any) {
      console.error("Error scheduling meeting", err);
      alert("❌ Failed to schedule meeting. It may already exist.");
    }
  };

  return (
    <>
      <NavBar />
      <div className="max-w-3xl mx-auto mt-12 px-6 py-8 bg-gradient-to-br from-white to-slate-100 shadow-2xl rounded-2xl border border-gray-200">
        <div className="flex items-center gap-3 mb-6">
          <CalendarCheck className="text-green-600 w-7 h-7" />
          <h2 className="text-3xl font-bold text-gray-800">Schedule a Meeting</h2>
        </div>

        {bookings.length === 0 ? (
          <div className="flex items-start gap-3 bg-red-50 border border-red-300 text-red-700 p-4 rounded-md mb-4">
            <AlertCircle className="w-6 h-6 mt-1" />
            <p className="font-medium text-sm">
              ⚠️ You must have an <strong>approved room booking</strong> with no existing meeting to schedule a new one.
            </p>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-5">
            {error && (
              <div className="bg-yellow-50 border border-yellow-300 text-yellow-700 p-3 rounded text-sm">
                {error}
              </div>
            )}

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Meeting Title
              </label>
              <input
                type="text"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-green-400 focus:outline-none"
                placeholder="e.g., Team Sync"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Agenda
              </label>
              <textarea
                value={agenda}
                onChange={(e) => setAgenda(e.target.value)}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-green-400 focus:outline-none"
                placeholder="e.g., Discuss sprint tasks, assign deliverables..."
                rows={4}
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Approved Booking
              </label>
              <select
                value={bookingID}
                onChange={(e) => setBookingID(e.target.value)}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-green-400 focus:outline-none"
                required
              >
                <option value="">Select Booking</option>
                {bookings.map((booking) => (
                  <option key={booking.bookingId} value={booking.bookingId}>
                    Room {booking.room.name} —{" "}
                    {new Date(booking.startTime).toLocaleString()} to{" "}
                    {new Date(booking.endTime).toLocaleString()}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Meeting Date & Time
              </label>
              <input
                type="datetime-local"
                value={dateTime}
                onChange={(e) => setDateTime(e.target.value)}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-green-400 focus:outline-none"
                required
              />
            </div>

            <button
              type="submit"
              className="w-full bg-green-600 hover:bg-green-700 text-white font-semibold py-2 rounded-lg shadow-sm transition-all duration-200"
            >
              Schedule Meeting
            </button>
          </form>
        )}
      </div>
    </>
  );
}
