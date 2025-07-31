import React from 'react';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import './App.css'
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import Dashboard from "./pages/Dashboard";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
//import CreateRoomPage from "./pages/CreateRoomPage"; // ✅ import it
import MeetingPage from './pages/MeetingPage';
import BookingPage from './pages/BookingPage';
import RoomPage from './pages/RoomPage';
import ProfileView from "./pages/Profile/ProfileView";
import ProfileEdit from "./pages/Profile/ProfileEdit";
import ChangePassword from "./pages/Profile/ChangePassword.tsx";
import CreateMoM from './pages/MoM/CreateMoM.tsx';
import ViewMoMs from './pages/MoM/ViewMoMs.tsx';
import CalendarPage from "./pages/CalendarPage";
import PendingBookings from './pages/PendingBookingsPage.tsx';

function App() {
  return (
    <Router>
      <>
      <Routes>
        <Route path="/" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/rooms" element={<RoomPage />}
        />
        <Route path="/meetings" element={<MeetingPage />} /> 
          
        <Route path="/bookings" element={<BookingPage />} />  
        <Route path="/profile" element={<ProfileView />} />
        <Route path="/profile/edit" element={<ProfileEdit />} />
        <Route path="/profile/change-password" element={<ChangePassword />} />
        <Route path="/mom/create" element={<CreateMoM />} />
        <Route path="/mom/view" element={<ViewMoMs />} />
        <Route path="/calendar" element={<CalendarPage />} />
        
        <Route path="/admin/pending-bookings" element={<PendingBookings />} />
 </Routes>
   <ToastContainer position="top-right" autoClose={3000} />

     
</>
    </Router>
  );
}

export default App
