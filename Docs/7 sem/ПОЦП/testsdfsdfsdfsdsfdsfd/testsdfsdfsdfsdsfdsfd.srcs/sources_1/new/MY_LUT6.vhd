----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2022 11:07:19
-- Design Name: 
-- Module Name: MY_LUT6 - Behavioral
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

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity MY_LUT6 is
    Port ( I : in STD_LOGIC_VECTOR (5 downto 0);
           O : out STD_LOGIC);
end MY_LUT6;

architecture Behavioral of MY_LUT6 is
component MY_LUT5 is
 generic (
   INIT: std_logic_vector(31 downto 0):=(others=>'0')
 );
 Port (
    I: in std_logic_Vector(4 downto 0);
    O: out  std_logic
  );
end component;
signal O1: std_logic;
signal O2: std_logic;
begin

U1: MY_LUT5 generic map (X"FFAD7635")
            port map (
                I=>I(4 downto 0),
                O=>O1
            );
U2: MY_LUT5 generic map (X"FFAD7635")
                        port map (
                            I=>I(4 downto 0),
                            O=>O2
                        );
O<= O1 when I(5) = '0' else O2;
end Behavioral;
