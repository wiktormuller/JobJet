import '../styles/main-styles.css';
import { Header } from "./Header.jsx";
import { Data } from  "./Data.jsx";
import { Map } from  "./Map.jsx";



export const Container = ({ userLogInState, setUserLogInState }) => {
    return (
      <div className="main-container">
        <Header
          userLogInState={userLogInState}
          setUserLogInState={setUserLogInState}
        ></Header>
        <div className="sub-container">
          <Data></Data>
          <Map></Map>
        </div>
      </div>
      );
}