----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2022 10:57:34
-- Design Name: 
-- Module Name: MY_LUT5 - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_LOGIC_ARITH.ALL;
use IEEE.STD_LOGIC_UNSIGNED.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity MY_LUT5 is
  generic (
    INIT: std_logic_vector(31 downto 0):=(others=>'0')
  );
  Port (
     I: in std_logic_Vector(4 downto 0);
     O: out  std_logic
   );
end MY_LUT5;

architecture Behavioral of MY_LUT5 is

begin
O<=INIT(CONV_INTEGER(I));
end Behavioral;
