import { Route, Routes } from "react-router-dom";
import DemoView from "./views/Demo/DemoView";
import RoomView from "./views/Room/RoomView";
import HomeView from "./views/Home/HomeView";

function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<HomeView />} />
        <Route path="/room" element={<RoomView />} />
        <Route path="/demo" element={<DemoView />} />
      </Routes>
    </div>
  );
}

export default App;
