import { Route, Routes } from "react-router-dom";
import Navbar from "./components/ui/Navbar";
import DemoView from "./views/Demo/DemoView";
import GameView from "./views/GameView";
import HomeView from "./views/Home/HomeView";

function App() {
  return (
    <div className="App">
      {/* <Navbar /> */}
      <Routes>
        <Route path="/" element={<HomeView />} />
        <Route path="/game" element={<GameView />} />
        <Route path="/demo" element={<DemoView />} />
      </Routes>
    </div>
  );
}

export default App;
