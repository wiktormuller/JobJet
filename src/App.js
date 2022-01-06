import "./styles/main-styles.css";

import { useState /*, useEffect*/ } from "react";
import { Navbar } from "./components/Navbar.jsx";
import { Container } from "./components/Container.jsx";

function App() {
  const [userLogInState, setUserLogInState] = useState(false);

  return (
    <div className="app">
      <Navbar></Navbar>
      <Container
        userLogInState={userLogInState}
        setUserLogInState={setUserLogInState}
      ></Container>
    </div>
  );
}

export default App;
