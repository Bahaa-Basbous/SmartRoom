interface Props {
  room: any;
  role: string;
  onEdit: () => void;
  onDelete: () => void;
}

export default function RoomCard({ room, role, onEdit, onDelete }: Props) {
  return (
    <div className="bg-white p-4 rounded-xl shadow hover:shadow-md transition">
      <h2 className="text-xl font-bold mb-1">{room.name}</h2>
      <p className="text-sm">Capacity: {room.capacity}</p>
      <p className="text-sm">Location: {room.location || "N/A"}</p>
      <p className="text-sm">Features: {room.features || "None"}</p>

      {role === "Admin" && (
        <div className="flex justify-end gap-2 mt-3">
          <button
            onClick={onEdit}
            className="bg-yellow-500 text-white px-3 py-1 rounded hover:bg-yellow-600"
          >
            Edit
          </button>
          <button
            onClick={onDelete}
            className="bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700"
          >
            Delete
          </button>
        </div>
      )}
    </div>
  );
}
