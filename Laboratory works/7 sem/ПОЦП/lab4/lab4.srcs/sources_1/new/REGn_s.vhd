----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 12:42:04 PM
-- Design Name: 
-- Module Name: REGn - Behavioral
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

entity REGn_s is
    Generic (n : integer := 4);
	Port (
	    EN : in std_logic;
		Din : in std_logic_vector(n-1 downto 0);
		Dout : out std_logic_vector(n-1 downto 0)
	);
end REGn_s;

architecture Structural of REGn_S is
    Component EDLatch is 
        Port ( E : in STD_LOGIC;
               D : in STD_LOGIC;
               Q : out STD_LOGIC);
    end component;
Begin
    gen0: for i in 0 to n-1 generate
        gen_i: EDLatch port map (E=>EN, D=>Din(i), Q=>Dout(i));
    end generate;
end Structural;
