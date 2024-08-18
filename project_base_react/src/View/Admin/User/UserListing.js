import MyTable from "../../../Control/MyTable"
import React from "react";
export default function UserListing(){
    const handleFetchData = (pageIndex, pageSize, sortBy, filters) => {
        console.log(pageSize);
    }

    const columns = React.useMemo(() => [
        {
            Header: 'ID',
            accessor: 'id',
        },
        {
            Header: 'Name',
            accessor: 'name',
        },
        {
            Header: 'Email',
            accessor: 'email',
        },
        {
            Header: 'Age',
            accessor: 'age',
        },
        {
            Header: 'City',
            accessor: 'city',
        },
        {
            Header: 'Active',
            accessor: 'active',
            Cell: ({ value }) => (value ? 'Yes' : 'No'),
        },
    ], []);
    
        const data = React.useMemo(() => [
            { id: 1, name: 'John Doe', email: 'john.doe@example.com', age: 28, city: 'New York', active: true },
            { id: 2, name: 'Jane Smith', email: 'jane.smith@example.com', age: 34, city: 'Los Angeles', active: false },
            { id: 3, name: 'Alice Johnson', email: 'alice.johnson@example.com', age: 29, city: 'Chicago', active: true },
            { id: 4, name: 'Bob Brown', email: 'bob.brown@example.com', age: 42, city: 'Houston', active: true },
            { id: 5, name: 'Carol White', email: 'carol.white@example.com', age: 31, city: 'Phoenix', active: false },
            { id: 6, name: 'David Black', email: 'david.black@example.com', age: 38, city: 'Philadelphia', active: true },
            { id: 7, name: 'Eva Green', email: 'eva.green@example.com', age: 27, city: 'San Antonio', active: true },
            { id: 8, name: 'Frank Harris', email: 'frank.harris@example.com', age: 45, city: 'San Diego', active: false },
            { id: 9, name: 'Grace Wilson', email: 'grace.wilson@example.com', age: 32, city: 'Dallas', active: true },
            { id: 10, name: 'Henry Miller', email: 'henry.miller@example.com', age: 40, city: 'San Jose', active: true },
        ], []);
        

    return <>
    <h1>User Listing</h1>
    <hr/>
    <MyTable 
    url={'/User/GetUserList'}
    fetchData={({pageIndex, pageSize, sortBy, filters}) => handleFetchData(pageIndex, pageSize, sortBy, filters)}
/></>
}