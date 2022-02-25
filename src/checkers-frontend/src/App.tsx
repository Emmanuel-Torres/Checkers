import { Route, Routes } from "react-router-dom";
import Navbar from "./components/ui/Navbar";
import { useStoreSelector } from "./store";
import Home from "./views/Home";

function App() {
  return (
    <div className="App">
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
      </Routes>
    </div>
  );
}

export default App;
