import '../styles/main-styles.css';
import '../styles/data-styles.css';

export const FilterData = () => {

    const filterArray = ["C/C++", "Java", "JavaScript", "SQL", "Java", "JavaScript", "SQL", "Java", "JavaScript", "SQL", "Java", "JavaScript", "SQL"];

    const renderedArray = filterArray.map(
      (x) => {
        return <button className="data__filter-data--btn">{x}</button>
      }
    )
    

    filterArray.map((x)=>{
      const str = x
      return `<div>${str}</div>`
    })

    return (
      <div className="data__filter-data">
        {renderedArray}
      </div>
      );
}