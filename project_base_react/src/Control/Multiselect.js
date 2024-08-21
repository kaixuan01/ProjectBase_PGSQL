import React from 'react';
import MultiSelect from 'multiselect-react-dropdown';

const CustomMultiSelect = ({ options, selectedValues, onChange }) => {
  // Convert options to the format required by multiselect-react-dropdown
  const formattedOptions = options.map(option => ({
    key: option.value,
    label: option.label,
  }));

  // Handle change event
  const handleSelectChange = (selectedList, selectedItem) => {
    const values = selectedList.map(item => item.key);
    onChange(values);
  };

  // Handle remove event
  const handleRemove = (selectedList, removedItem) => {
    const values = selectedList.map(item => item.key);
    onChange(values);
  };

  return (
    <MultiSelect
      options={formattedOptions}
      selectedValues={formattedOptions.filter(option => selectedValues.includes(option.key))}
      onSelect={handleSelectChange}
      onRemove={handleRemove}
      displayValue="label"
      showCheckbox
      placeholder=""
      hideSelectedList
    />
  );
};

export default CustomMultiSelect;
