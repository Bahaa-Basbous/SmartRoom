import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import listPlugin from "@fullcalendar/list";
import { useEffect, useState } from "react";
import axios from "../services/api";
import NavBar from "../components/NavBar";

import tippy from "tippy.js";
import "tippy.js/dist/tippy.css";

export default function CalendarPage() {
  const [events, setEvents] = useState<any[]>([]);

  useEffect(() => {
    axios.get("/Booking").then((res) => {
      const approvedBookings = res.data.filter((b: any) => b.status === "Approved");
      const formatted = approvedBookings.map((b: any) => ({
        title: b.room?.name || "Booking",
        start: b.startTime,
        end: b.endTime,
        allDay: false,
        extendedProps: {
          description: `${new Date(b.startTime).toLocaleTimeString([], {
            hour: "2-digit",
            minute: "2-digit",
          })} - ${b.room?.name}`,
        },
      }));
      setEvents(formatted);
    });
  }, []);

  // Attach tooltip on event mount with delay & arrow
  const handleEventDidMount = (info: any) => {
    tippy(info.el, {
      content: info.event.extendedProps.description,
      placement: "top",
      delay: [300, 100],
      arrow: true,
      theme: "light",
      animation: "shift-away",
      maxWidth: 220,
      allowHTML: true,
    });
  };

  return (
    <>
      <NavBar />
      <main className="max-w-6xl mx-auto mt-8 px-4 sm:px-6 lg:px-8">
        <header className="mb-8 flex items-center space-x-3">
          <span className="text-indigo-600 text-3xl">📅</span>
          <h1 className="text-3xl font-extrabold text-indigo-900 tracking-tight">
            Booking Calendar
          </h1>
        </header>

        <section className="bg-white rounded-lg shadow-md p-6">
          <FullCalendar
            plugins={[dayGridPlugin, timeGridPlugin, listPlugin]}
            initialView="timeGridWeek"
            events={events}
            height="auto"
            slotLabelFormat={{
              hour: "numeric",
              minute: "2-digit",
              hour12: true,
              meridiem: "short",
            }}
            eventTimeFormat={{
              hour: "numeric",
              minute: "2-digit",
              hour12: true,
              meridiem: "short",
            }}
            eventDidMount={handleEventDidMount}
            headerToolbar={{
              left: "prev,next today",
              center: "title",
              right: "dayGridMonth,timeGridWeek,listWeek",
            }}
            buttonText={{
              today: "Today",
              month: "Month",
              week: "Week",
              list: "List",
            }}
            dayMaxEventRows={true} // show +n when too many events
          />
        </section>
      </main>

      <style>{`
        /* Make day cells taller for better readability */
        .fc-daygrid-day-frame {
          min-height: 110px !important;
        }
        /* Truncate event titles elegantly */
        .fc-event-title {
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          max-width: 100%;
        }
        /* Customize tooltip */
        .tippy-box[data-theme~='light'] {
          background: #f9fafb;
          color: #1e293b;
          font-weight: 600;
          font-size: 0.875rem;
          box-shadow: 0 4px 8px rgb(0 0 0 / 0.1);
        }
      `}</style>
    </>
  );
}
