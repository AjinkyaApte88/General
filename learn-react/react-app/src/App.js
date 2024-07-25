import './App.css';
import {useEffect, useState, useReducer} from 'react';

function useClickInput(initialValue){
  const [value, setValue] = useState(initialValue);
  return [
    {
      value,
      onClick: (e) => setValue(e.target.value)
    },
    () => setValue(initialValue)
  ];
}

export function App() {
  const [status, setStatus] = useReducer((status)=>!status, false);
  const [condition, resetCondition] = useClickInput("empty");

  useEffect(()=>{console.log("App state changed!")});
  useEffect(()=> {console.log(status)}, [status]);
  useEffect(()=>{console.log(condition)}, [condition]);

  function ChangeStatus() {
    setStatus();
    resetCondition();
  }

  return (
    <div className="App">
      <h1 className='App-head'>Hello</h1>
      <h2>
        We are {status ? "open" : "closed"} !
      </h2>
      <input title='Status' type="checkbox" value={status} onChange={ChangeStatus} />Open
      <h2 className={status===false ? 'h2Disabled' : '' }>
        It's  {condition.value} right now!
      </h2>
      <button {...condition} value={"relaxed"} disabled={status===false}>Relaxed</button>
      <button {...condition} value={"busy"} disabled={status===false}>Busy</button>
    </div>
  );
}

export function Contact() {
  <div className='App'>
    <h1 className='App-head'>Contact Us</h1>
  </div>
}
