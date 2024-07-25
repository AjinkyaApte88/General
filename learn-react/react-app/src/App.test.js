import { fireEvent, render, screen } from '@testing-library/react';
import { App, Contact } from './App';

test('renders learn react link', () => {
  render(<App />);
  const linkElement = screen.getByText(/hello/i);
  expect(linkElement).toBeInTheDocument();
});

test('checkbox changes value when clicked', () => {
  render(<App />);
  const linkElement = screen.getByTitle(/Status/i);
  const before = linkElement.checked;
  fireEvent.click(linkElement);
  expect(linkElement.checked).toEqual(!before);
});

