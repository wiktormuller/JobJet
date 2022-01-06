import '../styles/main-styles.css';
import { InputData } from "./InputData";
import { FilterData } from "./FilterData";


export const Data = () => {
    return (
      <div className="data">
        <InputData></InputData>
        <FilterData></FilterData>
      </div>
      );
}