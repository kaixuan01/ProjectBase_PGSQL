import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { initData, updateData, deleteData } from '../Redux/actions';

const Dashboard = () => {
  const dispatch = useDispatch();
  useSelector((state) => {console.log(state)});
  console.log(123);
  
  useEffect(() => {
    // Example of initializing data
    console.log(123)
    dispatch(initData('user', [{ id: 1, name: 'John Doe' }]));
  }, [dispatch]);

  const handleUpdateUser = (id, name) => {
    dispatch(updateData('user', id, { name }));
  };

  const handleDeleteUser = (id) => {
    dispatch(deleteData('user', id));
  };

  return (
    <div>
      <h1>User Profile</h1>
      {/* {users.map((user) => (
        <div key={user.id}>
          <p>Name: {user.name}</p>
          <button onClick={() => handleUpdateUser(user.id, 'Jane Doe')}>
            Update User
          </button>
          <button onClick={() => handleDeleteUser(user.id)}>Delete User</button>
        </div>
      ))} */}
    </div>
  );
};

export default Dashboard;
