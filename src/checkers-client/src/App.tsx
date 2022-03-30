import { Route, Routes } from "react-router-dom";
import Navbar from "./components/ui/Navbar";
import DemoView from "./views/DemoView";
import GameView from "./views/GameView";
import HomeView from "./views/HomeView";
import LoginView from "./views/LoginView";

function App() {
  return (
    <div className="App">
      <Navbar />
      <Routes>
        <Route path="/" element={<HomeView />} />
        <Route path="/game" element={<GameView />} />
        <Route path="/login" element={<LoginView />} />
        <Route path="/demo" element={<DemoView />} />
      </Routes>
    </div>
  );
}

export default App;
