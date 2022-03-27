import { Route, Routes } from "react-router-dom";
import Navbar from "./components/ui/Navbar";
import Home from "./views/Home";
import Secure from "./views/Secure";

function App() {
  return (
    <div className="App">
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/secure" element={<Secure />} />
      </Routes>
    </div>
  );
}

export default App;
