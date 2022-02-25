import Login from "./components/auth/Login";
import Logout from "./components/auth/Logout";
import { useStoreSelector } from "./store";

function App() {
  const token = useStoreSelector(store => store.auth.userToken);

  return (
    <div className="App">
      <Login />
      <Logout />
      {/* <h2>{token}</h2> */}
    </div>
  );
}

export default App;
