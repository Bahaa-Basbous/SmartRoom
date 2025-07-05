import React from 'react';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import './App.css'
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import Dashboard from "./pages/Dashboard";
import CreateRoomPage from "./pages/CreateRoomPage"; // ✅ import it

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/create-room" element={<CreateRoomPage />} /> {/* ✅ add this route */}
        {/* Add more routes as needed */}
      </Routes>
    </Router>
  );
}

export default App
