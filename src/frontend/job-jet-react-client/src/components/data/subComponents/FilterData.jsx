import "../../../styles/main-styles.css";
import "../styles/data-styles.css";

import { FilterButton } from "./FilterButton";

export const FilterData = (props) => {
  const renderedArray = props.skillsArray.map((skill) => {
    return (
      <FilterButton
        key={skill.id}
        id={skill.id}
        name={skill.name}
        icon={skill.icon}
        color={skill.color}
        searchedSkills={props.searchedSkills}
        setSearchedSkills={props.setSearchedSkills}
        filterData={props.filterData}
      />
    );
  });

  return <div className="data__filter-data">{renderedArray}</div>;
};
