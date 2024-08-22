import { useDispatch } from 'react-redux';
import { updateData } from '../Redux/actions';

export const useAuthHandlers = () => {
  const dispatch = useDispatch();

  const handleLogin = () => {
    dispatch(updateData('isLogin', 'Login'));
  };

  const handleLogout = () => {
    dispatch(updateData('isLogin', 'NotLogin'));
  };

  return { handleLogin, handleLogout };
};
