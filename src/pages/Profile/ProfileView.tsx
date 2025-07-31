import { useEffect, useState } from "react";
import axios from "../../services/api";
import { useNavigate } from "react-router-dom";
import { FaUserEdit, FaSignOutAlt, FaLock, FaArrowLeft } from "react-icons/fa";

export default function ProfileView() {
  const [profile, setProfile] = useState<any>(null);
  const navigate = useNavigate();

  useEffect(() => {
    axios.get("/User/me")
      .then((res) => setProfile(res.data))
      .catch(() => {
        alert("Failed to load profile. Please login again.");
        navigate("/");
      });
  }, [navigate]);

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
  };

  const goToDashboard = () => {
    navigate("/dashboard");
  };

  if (!profile) return <div className="p-4">Loading profile...</div>;

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-gray-100">
      <div className="w-full max-w-md bg-white p-8 rounded-2xl shadow-xl border border-gray-200">
        <h2 className="text-3xl font-bold text-center text-blue-700 mb-6">
          My Profile
        </h2>

        <div className="space-y-4 text-gray-700 text-lg">
          <div className="flex justify-between">
            <span className="font-semibold">Name:</span>
            <span>{profile.name}</span>
          </div>
          <div className="flex justify-between">
            <span className="font-semibold">Email:</span>
            <span>{profile.email}</span>
          </div>
          <div className="flex justify-between">
            <span className="font-semibold">Role:</span>
            <span className="capitalize">{profile.role}</span>
          </div>
        </div>

        <div className="mt-8 grid grid-cols-2 gap-4">
          <button
            onClick={() => navigate("/profile/edit")}
            className="flex items-center justify-center gap-2 bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-lg transition"
          >
            <FaUserEdit /> Edit
          </button>
          <button
            onClick={() => navigate("/profile/change-password")}
            className="flex items-center justify-center gap-2 bg-yellow-500 hover:bg-yellow-600 text-white font-medium py-2 px-4 rounded-lg transition"
          >
            <FaLock /> Reset
          </button>
          <button
            onClick={goToDashboard}
            className="col-span-2 flex items-center justify-center gap-2 bg-gray-500 hover:bg-gray-600 text-white font-medium py-2 px-4 rounded-lg transition"
          >
            <FaArrowLeft /> Back to Dashboard
          </button>
          <button
            onClick={handleLogout}
            className="col-span-2 flex items-center justify-center gap-2 bg-red-600 hover:bg-red-700 text-white font-medium py-2 px-4 rounded-lg transition"
          >
            <FaSignOutAlt /> Logout
          </button>
        </div>
      </div>
    </div>
  );
}
